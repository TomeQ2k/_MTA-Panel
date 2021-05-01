using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Application.Services.Sdks;
using MTA.Core.Application.SmartEnums;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

#pragma warning disable 8632

namespace MTA.Infrastructure.Shared.Services
{
    public class PremiumAccountManager : IPremiumAccountManager
    {
        private readonly IDatabase database;
        private readonly ICharacterService characterService;
        private readonly IReadOnlyUserService userService;
        private readonly IPurchaseService purchaseService;
        private readonly IAdminActionService adminActionService;
        private readonly IEmailSender emailSender;
        private readonly IMtaManager mtaManager;
        private readonly IOrderService orderService;
        private readonly IPremiumUserLibraryManager premiumUserLibraryManager;
        private readonly ITempDatabaseCleaner tempDatabaseCleaner;
        private readonly IFilesManager filesManager;
        private readonly ICustomInteriorManager customInteriorManager;
        private readonly IHttpContextReader httpContextReader;

        public PremiumAccountManager(IDatabase database, ICharacterService characterService,
            IReadOnlyUserService userService, IPurchaseService purchaseService, IAdminActionService adminActionService,
            IEmailSender emailSender, IMtaManager mtaManager, IOrderService orderService,
            IPremiumUserLibraryManager premiumUserLibraryManager, ITempDatabaseCleaner tempDatabaseCleaner,
            IFilesManager filesManager, ICustomInteriorManager customInteriorManager,
            IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.characterService = characterService;
            this.userService = userService;
            this.purchaseService = purchaseService;
            this.adminActionService = adminActionService;
            this.emailSender = emailSender;
            this.mtaManager = mtaManager;
            this.orderService = orderService;
            this.premiumUserLibraryManager = premiumUserLibraryManager;
            this.tempDatabaseCleaner = tempDatabaseCleaner;
            this.filesManager = filesManager;
            this.customInteriorManager = customInteriorManager;
            this.httpContextReader = httpContextReader;
        }

        public async Task<ObjectProtectionResult> AddObjectProtection(AddObjectProtectionRequest request)
        {
            var accountCharacters =
                await database.CharacterRepository.GetAccountCharactersWithEstatesAndVehicles(httpContextReader
                    .CurrentUserId);

            if (!accountCharacters.Any())
                return new ObjectProtectionResult(false, request.ProtectionType, request.ObjectId);

            using (var transaction = database.BeginTransaction().Transaction)
            {
                var objectProtectionResult = await ObjectProtectionSmartEnum.FromValue((int) request.ProtectionType)
                    .ProtectObject(request.ObjectId, request.Amount, accountCharacters, database);

                if (!objectProtectionResult.IsSucceeded)
                    return objectProtectionResult;

                if (await purchaseService.CreatePurchase(PremiumConstants.AddObjectProtection,
                    httpContextReader.CurrentUserId, request.Amount * PremiumConstants.AddObjectProtectionCost) == null)
                    throw new PremiumOperationException(
                        "Adding object protection failed");

                transaction.Complete();

                return new ObjectProtectionResult(true, request.ProtectionType, request.ObjectId,
                    objectProtectionResult.ProtectedUntil);
            }
        }

        public async Task<AddSerialSlotResult> AddSerialSlot(AddSerialSlotRequest request)
        {
            var user = await database.UserRepository.FindById(httpContextReader.CurrentUserId) ??
                       throw new EntityNotFoundException("User not found");

            user.IncreaseSerialsLimit(request.Amount);

            using (var transaction = database.BeginTransaction().Transaction)
            {
                if (!await database.UserRepository.Update(user))
                    throw new PremiumOperationException(
                        "Adding serial slot failed");

                if (await purchaseService.CreatePurchase(PremiumConstants.AddSerialSlot,
                    httpContextReader.CurrentUserId, request.Amount * PremiumConstants.AddSerialSlotCost) == null)
                    throw new PremiumOperationException(
                        "Adding serial slot failed");

                transaction.Complete();
            }

            return new AddSerialSlotResult(true, user.SerialsLimit);
        }

        public async Task<bool> AddCustomSkin(IFormFile skinFile, int skinId, int characterId)
        {
            if (await database.CharacterRepository.Find(new SqlBuilder()
                .Append("account").Equals.Append(httpContextReader.CurrentUserId).And
                .Append("id").Equals.Append(characterId).Build().Query) == null)
                throw new DatabaseException("Character does not belong to you");

            if (skinFile == null)
                throw new ServerException("Skin file does not exists");

            var order = new OrderBuilder()
                .SetOperation(PremiumOperation.Skin)
                .SetCost(PremiumConstants.AddCustomSkinCost)
                .SetUser(httpContextReader.CurrentUserId)
                .SetCharacter(characterId)
                .Build();

            using (var transaction = database.BeginTransaction().Transaction)
            {
                if (await orderService.CreateOrder(order) == null)
                    throw new DatabaseException("Creating order failed");

                if (await premiumUserLibraryManager.AddFileToLibrary(skinFile, PremiumFileType.Skin, order.Id,
                    skinId) == null)
                    throw new DatabaseException("Adding skin file to library failed");

                if (await purchaseService.CreatePurchase(PremiumConstants.AddCustomSkin,
                    httpContextReader.CurrentUserId, PremiumConstants.AddCustomSkinCost) == null)
                    throw new PremiumOperationException(
                        "Adding custom skin failed");

                transaction.Complete();
            }

            return true;
        }

        public async Task<bool> AddCustomInterior(IFormFile interiorFile, int estateId)
        {
            if (interiorFile == null)
                throw new ServerException("Interior file does not exists");

            var user = await userService.GetUserWithCharacters(httpContextReader.CurrentUserId) ??
                       throw new EntityNotFoundException("User not found");

            var estate = await characterService.HasAnyCharacterEstate(user.Characters, estateId) ??
                         throw new NoPermissionsException("You are not owner of this estate");

            using (var transaction = database.BeginTransaction().Transaction)
            {
                await tempDatabaseCleaner.ClearGameTempObjectsAndInteriors();

                var order = new OrderBuilder()
                    .SetOperation(PremiumOperation.Interior)
                    .SetCost(PremiumConstants.AddCustomInteriorCost)
                    .SetUser(httpContextReader.CurrentUserId)
                    .SetEstate(estateId)
                    .Build();

                if (await orderService.CreateOrder(order) == null)
                    throw new DatabaseException("Inserting order failed");

                var premiumFile =
                    await premiumUserLibraryManager.AddFileToLibrary(interiorFile, PremiumFileType.Interior,
                        order.Id) ??
                    throw new DatabaseException("Adding interior file to library failed");

                await filesManager.ReplaceInFile(premiumFile.Path, "edf:", string.Empty);

                var (gameTempObjects, gameTempInterior) =
                    customInteriorManager.InitGameTempObjectsAndInteriors(estate, premiumFile);

                await customInteriorManager.ExecuteAddCustomInterior(premiumFile, gameTempObjects, gameTempInterior);

                if (await purchaseService.CreatePurchase(PremiumConstants.AddCustomInterior,
                    httpContextReader.CurrentUserId, PremiumConstants.AddCustomInteriorCost) == null)
                    throw new PremiumOperationException(
                        "Adding custom skin failed");

                transaction.Complete();
            }

            return true;
        }

        public async Task<bool> TransferCharacter(int sourceCharacterId, int targetCharacterId)
        {
            var user = await userService.GetUserWithCharacters(httpContextReader.CurrentUserId) ??
                       throw new EntityNotFoundException("User not found");

            var (sourceCharacter, targetCharacter) =
                (user.Characters.FirstOrDefault(c => c.Id == sourceCharacterId) ??
                 throw new EntityNotFoundException("Character not found"),
                    user.Characters.FirstOrDefault(c => c.Id == targetCharacterId) ??
                    throw new EntityNotFoundException("Character not found"));

            using (var transaction = database.BeginTransaction().Transaction)
            {
                await ExecuteTransferCharacter(sourceCharacter, targetCharacter);

                transaction.Complete();
            }

            try
            {
                await emailSender.Send(EmailMessages.TransferCharacterEmail(user.Email));

                mtaManager.CallFunction(MtaResources.WebsiteHttpFunctions, MtaFunctions.TransferCharacter);
            }
            catch (MTARequestException)
            {
            }

            return true;
        }

        #region private

        private async Task ExecuteTransferCharacter(Character? sourceCharacter, Character? targetCharacter)
        {
            if (!await characterService.TransferMoney(sourceCharacter, targetCharacter))
                throw new PremiumOperationException(
                    $"Transfering money from character #{sourceCharacter.Id} to character #{targetCharacter.Id} failed");

            if (!await characterService.TransferEstatesAndVehicles(sourceCharacter.Estates,
                sourceCharacter.Vehicles, targetCharacter.Id))
                throw new PremiumOperationException(
                    $"Transfering estates and vehicles from character #{sourceCharacter.Id} to character #{targetCharacter.Id} failed");

            if (!await characterService.TransferGameItems(sourceCharacter, targetCharacter))
                throw new PremiumOperationException(
                    $"Transfering game items from character #{sourceCharacter.Id} to character #{targetCharacter.Id} failed");

            if ((await adminActionService.InsertAdminAction(new AdminActionBuilder()
                .SetUserId(httpContextReader.CurrentUserId)
                .SetReason(
                    $"Transfer character data from character #{sourceCharacter.Id} to character #{targetCharacter.Id}")
                .Build()) == null))
                throw new PremiumOperationException(
                    $"Transfering character data from character #{sourceCharacter.Id} to character #{targetCharacter.Id} failed");

            if (await purchaseService.CreatePurchase(PremiumConstants.TransferCharacter,
                httpContextReader.CurrentUserId, PremiumConstants.TransferCharacterCost) == null)
                throw new PremiumOperationException(
                    $"Transfering character data from character #{sourceCharacter.Id} to character #{targetCharacter.Id} failed");
        }

        #endregion
    }
}