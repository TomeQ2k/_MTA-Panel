using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Persistence.Database;
using MTA.Infrastructure.Shared.Services;
using MTA.UnitTests.TestModels;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class PremiumUserLibraryManagerTests
    {
        private PremiumUserLibraryManager premiumUserLibraryManager;

        private Mock<IFilesManager> filesManager;
        private Mock<IDatabase> database;
        private Mock<ITempDatabaseCleaner> tempDatabaseCleaner;
        private Mock<ICustomInteriorManager> customInteriorManager;
        private Mock<IHttpContextReader> httpContextReader;

        private FileModel uploadedFile = new FileModel(FilePath, It.IsNotNull<string>(), 1000);

        private IFormFile file = new FormFile(It.IsNotNull<Stream>(), It.IsNotNull<long>(),
            It.IsNotNull<long>(),
            It.IsNotNull<string>(), It.IsNotNull<string>());

        private const string LibraryPath = "library";
        private const string FilePath = LibraryPath + "/file";
        private const string OrderId = "id";
        private const string OldFileId = "oldId";
        private const int UserId = 1;

        [SetUp]
        public void SetUp()
        {
            filesManager = new Mock<IFilesManager>();
            database = new Mock<IDatabase>();
            tempDatabaseCleaner = new Mock<ITempDatabaseCleaner>();
            customInteriorManager = new Mock<ICustomInteriorManager>();
            httpContextReader = new Mock<IHttpContextReader>();

            filesManager.Setup(f => f.Upload(It.IsNotNull<IFormFile>(), It.IsNotNull<string>()))
                .ReturnsAsync(uploadedFile);
            database.Setup(d => d.PremiumFileRepository.Insert(It.IsNotNull<PremiumFile>(), false)).ReturnsAsync(true);
            database.Setup(d => d.PremiumFileRepository.GetFileWithOrder(OldFileId)).ReturnsAsync(() =>
            {
                var premiumFile = new PremiumFile();
                premiumFile.SetOrder(new Order());
                return premiumFile;
            });
            database.Setup(d => d.PremiumFileRepository.GetFileWithOrderAndEstate(OldFileId)).ReturnsAsync(() =>
            {
                var premiumFile = new PremiumFile();
                premiumFile.SetOrder(new Order());
                premiumFile.SetEstate(new Estate());
                return premiumFile;
            });
            database.Setup(d => d.BeginTransaction()).Returns(new DatabaseTransaction());
            database.Setup(d => d.PremiumFileRepository.Delete(It.IsNotNull<PremiumFile>())).ReturnsAsync(true);
            database.Setup(d => d.OrderRepository.Update(It.IsNotNull<Order>())).ReturnsAsync(true);
            customInteriorManager.Setup(c =>
                    c.InitGameTempObjectsAndInteriors(It.IsNotNull<Estate>(), It.IsNotNull<PremiumFile>()))
                .Returns((new List<GameTempObject>(), new GameTempInterior()));
            httpContextReader.Setup(h => h.CurrentUserId).Returns(UserId);

            premiumUserLibraryManager = new PremiumUserLibraryManager(filesManager.Object, database.Object,
                tempDatabaseCleaner.Object, customInteriorManager.Object, httpContextReader.Object);
        }

        #region AddFileToLibrary

        [Test]
        public void AddFileToLibrary_FileNotExist_ThrowServerException()
        {
            Assert.That(() => premiumUserLibraryManager.AddFileToLibrary(null, PremiumFileType.Interior, OrderId),
                Throws.Exception.TypeOf<ServerException>());
        }

        [Test]
        public void AddFileToLibrary_InsertingFileToDatabaseFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.PremiumFileRepository.Insert(It.IsNotNull<PremiumFile>(), false)).ReturnsAsync(false);

            Assert.That(() => premiumUserLibraryManager.AddFileToLibrary(file, PremiumFileType.Interior, OrderId),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void AddFileToLibrary_InsertingFileToDatabaseFailed_DeleteFileShouldBeCalled()
        {
            database.Setup(d => d.PremiumFileRepository.Insert(It.IsNotNull<PremiumFile>(), false)).ReturnsAsync(false);

            premiumUserLibraryManager.AddFileToLibrary(file, PremiumFileType.Interior, OrderId);

            filesManager.Verify(f => f.DeleteByFullPath(FilePath), Times.Once);
        }

        [Test]
        public async Task AddFileToLibrary_WhenCalled_ReturnPremiumFile()
        {
            var result = await premiumUserLibraryManager.AddFileToLibrary(file, PremiumFileType.Interior, OrderId);

            Assert.That(result, Is.Not.Null
                .And.TypeOf<PremiumFile>());
        }

        #endregion

        #region ChangeUploadedSkinFile

        [Test]
        public void ChangeUploadedSkinFile_NewFileNotExists_ThrowServerException()
        {
            Assert.That(() => premiumUserLibraryManager.ChangeUploadedSkinFile(null, OldFileId),
                Throws.Exception.TypeOf<ServerException>());
        }

        [Test]
        public void ChangeUploadedSkinFile_OldPremiumFileNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.PremiumFileRepository.GetFileWithOrder(OldFileId)).ReturnsAsync(() => null);

            Assert.That(() => premiumUserLibraryManager.ChangeUploadedSkinFile(file, OldFileId),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void ChangeUploadedSkinFile_OldPremiumFileTimeToChangeExpired_ThrowNoPermissionsException()
        {
            database.Setup(d => d.PremiumFileRepository.GetFileWithOrder(OldFileId))
                .ReturnsAsync(() =>
                {
                    var premiumFile = new TestPremiumFile();
                    premiumFile.SetDateCreated(-3);
                    return premiumFile;
                });

            Assert.That(() => premiumUserLibraryManager.ChangeUploadedSkinFile(file, OldFileId),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void ChangeUploadedSkinFile_AddingFileToLibraryFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.PremiumFileRepository.Insert(It.IsNotNull<PremiumFile>(), false)).ReturnsAsync(false);

            Assert.That(() => premiumUserLibraryManager.ChangeUploadedSkinFile(file, OldFileId),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void ChangeUploadedSkinFile_DeletingOldPremiumFileFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.PremiumFileRepository.Delete(It.IsNotNull<PremiumFile>())).ReturnsAsync(false);

            Assert.That(() => premiumUserLibraryManager.ChangeUploadedSkinFile(file, OldFileId),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void ChangeUploadedSkinFile_UpdatingOrderFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.OrderRepository.Update(It.IsNotNull<Order>())).ReturnsAsync(false);

            Assert.That(() => premiumUserLibraryManager.ChangeUploadedSkinFile(file, OldFileId),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void ChangeUploadedSkinFile_WhenCalled_DeleteOldFileShouldBeCalled()
        {
            premiumUserLibraryManager.ChangeUploadedSkinFile(file, OldFileId);

            filesManager.Verify(f => f.DeleteByFullPath(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ChangeUploadedSkinFile_WhenCalled_ReturnTrue()
        {
            var result = await premiumUserLibraryManager.ChangeUploadedSkinFile(file, OldFileId);

            Assert.That(result, Is.True);
        }

        #endregion

        #region ChangeUploadedInteriorFile

        [Test]
        public void ChangeUploadedInteriorFile_NewFileNotExists_ThrowServerException()
        {
            Assert.That(() => premiumUserLibraryManager.ChangeUploadedInteriorFile(null, OldFileId),
                Throws.Exception.TypeOf<ServerException>());
        }

        [Test]
        public void ChangeUploadedInteriorFile_OldPremiumFileNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.PremiumFileRepository.GetFileWithOrderAndEstate(OldFileId)).ReturnsAsync(() => null);

            Assert.That(() => premiumUserLibraryManager.ChangeUploadedInteriorFile(file, OldFileId),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void ChangeUploadedInteriorFile_OldPremiumFileTimeToChangeExpired_ThrowNoPermissionsException()
        {
            database.Setup(d => d.PremiumFileRepository.GetFileWithOrderAndEstate(OldFileId))
                .ReturnsAsync(() =>
                {
                    var premiumFile = new TestPremiumFile();
                    premiumFile.SetDateCreated(-3);
                    return premiumFile;
                });

            Assert.That(() => premiumUserLibraryManager.ChangeUploadedInteriorFile(file, OldFileId),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void ChangeUploadedInteriorFile_WhenCalled_()
        {
            premiumUserLibraryManager.ChangeUploadedSkinFile(file, OldFileId);

            filesManager.Verify(f => f.DeleteByFullPath(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void ChangeUploadedInteriorFile_DeletingOldPremiumFileFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.PremiumFileRepository.Delete(It.IsNotNull<PremiumFile>())).ReturnsAsync(false);

            Assert.That(() => premiumUserLibraryManager.ChangeUploadedInteriorFile(file, OldFileId),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void ChangeUploadedInteriorFile_UpdatingOrderFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.OrderRepository.Update(It.IsNotNull<Order>())).ReturnsAsync(false);

            Assert.That(() => premiumUserLibraryManager.ChangeUploadedInteriorFile(file, OldFileId),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void ChangeUploadedInteriorFile_AddingFileToLibraryFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.PremiumFileRepository.Insert(It.IsNotNull<PremiumFile>(), false)).ReturnsAsync(false);

            Assert.That(() => premiumUserLibraryManager.ChangeUploadedInteriorFile(file, OldFileId),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void ChangeUploadedInteriorFile_WhenCalled_ReplaceInFileShouldBeCalled()
        {
            premiumUserLibraryManager.ChangeUploadedInteriorFile(file, OldFileId);

            filesManager.Verify(f => f.ReplaceInFile(It.IsAny<string>(), "edf:", string.Empty), Times.Once);
        }

        [Test]
        public void ChangeUploadedInteriorFile_WhenCalled_InitGameTempObjectsAndInteriorsShouldBeCalled()
        {
            premiumUserLibraryManager.ChangeUploadedInteriorFile(file, OldFileId);

            customInteriorManager.Verify(
                c => c.InitGameTempObjectsAndInteriors(It.IsNotNull<Estate>(), It.IsNotNull<PremiumFile>()),
                Times.Once);
        }

        [Test]
        public void ChangeUploadedInteriorFile_WhenCalled_ExecuteAddCustomInteriorShouldBeCalled()
        {
            premiumUserLibraryManager.ChangeUploadedInteriorFile(file, OldFileId);

            customInteriorManager.Verify(
                c => c.InitGameTempObjectsAndInteriors(It.IsNotNull<Estate>(), It.IsNotNull<PremiumFile>()),
                Times.Once);
        }

        [Test]
        public void ChangeUploadedInteriorFile_WhenCalled_DeleteOldFileShouldBeCalled()
        {
            premiumUserLibraryManager.ChangeUploadedInteriorFile(file, OldFileId);

            filesManager.Verify(f => f.DeleteByFullPath(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ChangeUploadedInteriorFile_WhenCalled_ReturnTrue()
        {
            var result = await premiumUserLibraryManager.ChangeUploadedInteriorFile(file, OldFileId);

            Assert.That(result, Is.True);
        }

        #endregion
    }
}