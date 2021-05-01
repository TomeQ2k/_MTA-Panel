using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class PremiumUserLibraryManager : IPremiumUserLibraryManager
    {
        private readonly IFilesManager filesManager;
        private readonly IDatabase database;
        private readonly ITempDatabaseCleaner tempDatabaseCleaner;
        private readonly ICustomInteriorManager customInteriorManager;
        private readonly IHttpContextReader httpContextReader;

        public PremiumUserLibraryManager(IFilesManager filesManager, IDatabase database,
            ITempDatabaseCleaner tempDatabaseCleaner, ICustomInteriorManager customInteriorManager,
            IHttpContextReader httpContextReader)
        {
            this.filesManager = filesManager;
            this.database = database;
            this.tempDatabaseCleaner = tempDatabaseCleaner;
            this.customInteriorManager = customInteriorManager;
            this.httpContextReader = httpContextReader;
        }

        public async Task<PremiumUserLibraryResult> FetchLibraryFiles()
        {
            var libraryFiles = await database.PremiumFileRepository.GetWhere(new SqlBuilder()
                .Append("userId").Equals.Append(httpContextReader.CurrentUserId)
                .Build().Query);

            return new PremiumUserLibraryResult(
                libraryFiles.Where(lf => lf.FileType == (int) PremiumFileType.Skin),
                libraryFiles.Where(lf => lf.FileType == (int) PremiumFileType.Interior));
        }

        public async Task<PremiumFile> AddFileToLibrary(IFormFile file, PremiumFileType type, string orderId,
            int? skinId = null)
        {
            if (file == null)
                throw new ServerException("File does not exists");

            string fileLibraryPath =
                $"premium/library/{httpContextReader.CurrentUserId}/{Enum.GetName(type)?.ToLower()}s/";

            var uploadedFile = await filesManager.Upload(file, fileLibraryPath);

            var premiumFile = BaseFile.Create<PremiumFile>(uploadedFile.Url, uploadedFile.Path)
                .SetOrderId(orderId)
                .SetSkin(skinId)
                .SetFileType(type)
                .SetUserId(httpContextReader.CurrentUserId);

            if (await database.PremiumFileRepository.Insert(premiumFile, false))
                return premiumFile;
            else
            {
                filesManager.DeleteByFullPath(uploadedFile.Path);
                throw new DatabaseException("Adding order file to database failed");
            }
        }

        //TODO dodac lua script przy zmianie plikow + default
        public async Task<bool> ChangeUploadedSkinFile(IFormFile newFile, string oldFileId, int? skinId = null)
        {
            if (newFile == null)
                throw new ServerException("File does not exist");

            var oldPremiumFile = await database.PremiumFileRepository.GetFileWithOrder(oldFileId)
                                 ?? throw new EntityNotFoundException("Premium file not found");

            ValidateOldFileExpiration(oldPremiumFile);

            using (var transaction = database.BeginTransaction().Transaction)
            {
                await DeleteOldPremiumFileAndUpdateOrder(oldPremiumFile);

                if (await AddFileToLibrary(newFile, (PremiumFileType) oldPremiumFile.FileType,
                    oldPremiumFile.OrderId, skinId) == null)
                    throw new DatabaseException("Adding new skin file to premium library failed");

                transaction.Complete();
            }

            filesManager.DeleteByFullPath(oldPremiumFile.Path);

            return true;
        }

        public async Task<bool> ChangeUploadedInteriorFile(IFormFile newFile, string oldFileId)
        {
            if (newFile == null)
                throw new ServerException("File does not exist");

            var oldPremiumFile = await database.PremiumFileRepository.GetFileWithOrderAndEstate(oldFileId)
                                 ?? throw new EntityNotFoundException("Premium file not found");

            ValidateOldFileExpiration(oldPremiumFile);

            using (var transaction = database.BeginTransaction().Transaction)
            {
                await tempDatabaseCleaner.ClearGameTempObjectsAndInteriors();

                await DeleteOldPremiumFileAndUpdateOrder(oldPremiumFile);

                var newPremiumFile = await AddFileToLibrary(newFile, (PremiumFileType) oldPremiumFile.FileType,
                                         oldPremiumFile.OrderId) ??
                                     throw new DatabaseException("Adding new interior file to premium library failed");

                await filesManager.ReplaceInFile(newPremiumFile.Path, "edf:", string.Empty);

                var (gameTempObjects, gameTempInterior) =
                    customInteriorManager.InitGameTempObjectsAndInteriors(oldPremiumFile.Estate, newPremiumFile);

                await customInteriorManager.ExecuteAddCustomInterior(newPremiumFile, gameTempObjects, gameTempInterior);

                transaction.Complete();
            }

            filesManager.DeleteByFullPath(oldPremiumFile.Path);

            return true;
        }


        #region private

        private void ValidateOldFileExpiration(PremiumFile oldPremiumFile)
        {
            if (oldPremiumFile.DateCreated.AddDays(1) < DateTime.Now)
                throw new NoPermissionsException("Time to change file has expired");

            oldPremiumFile.Order.SetApprovalState(StateType.None);
        }

        private async Task DeleteOldPremiumFileAndUpdateOrder(PremiumFile oldPremiumFile)
        {
            if (!await database.PremiumFileRepository.Delete(oldPremiumFile))
                throw new DatabaseException("Deleting old file failed");

            if (!await database.OrderRepository.Update(oldPremiumFile.Order))
                throw new DatabaseException();
        }

        #endregion
    }
}