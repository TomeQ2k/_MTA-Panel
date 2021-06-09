using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.Sdks;
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
    public class PremiumAccountManagerTests
    {
        private PremiumAccountManager premiumAccountManager;

        private Mock<IDatabase> database;
        private Mock<ICharacterService> characterService;
        private Mock<IUserService> userService;
        private Mock<IPurchaseService> purchaseService;
        private Mock<IAdminActionService> adminActionService;
        private Mock<IEmailSender> emailSender;
        private Mock<IMtaManager> mtaManager;
        private Mock<IOrderService> orderService;
        private Mock<IPremiumUserLibraryManager> premiumUserLibraryManager;
        private Mock<ITempDatabaseCleaner> tempDatabaseCleaner;
        private Mock<IFilesManager> filesManager;
        private Mock<ICustomInteriorManager> customInteriorManager;
        private Mock<IHttpContextReader> httpContextReader;

        private AddObjectProtectionRequest addObjectProtectionRequest;
        private AddSerialSlotRequest addSerialSlotRequest;
        private Character sourceCharacter;
        private Character targetCharacter;
        private Estate estate;
        private FormFile file;

        [SetUp]
        public void SetUp()
        {
            estate = new Estate();
            var user = new User();
            sourceCharacter = new Character();
            targetCharacter = new Character();
            user.Characters.Add(new TestCharacter().SetEstate(estate));
            user.Characters.Add(sourceCharacter);
            user.Characters.Add(targetCharacter);

            file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data",
                "dummy.txt");

            addObjectProtectionRequest = new AddObjectProtectionRequest
            {
                ProtectionType = ObjectProtectionType.Estate,
                ObjectId = estate.Id,
                Amount = 1
            };

            addSerialSlotRequest = new AddSerialSlotRequest
            {
                Amount = 1
            };

            var characters = new List<Character>
            {
                new TestCharacter().SetEstate(estate),
                new TestCharacter()
            };

            database = new Mock<IDatabase>();
            characterService = new Mock<ICharacterService>();
            userService = new Mock<IUserService>();
            purchaseService = new Mock<IPurchaseService>();
            adminActionService = new Mock<IAdminActionService>();
            emailSender = new Mock<IEmailSender>();
            mtaManager = new Mock<IMtaManager>();
            orderService = new Mock<IOrderService>();
            premiumUserLibraryManager = new Mock<IPremiumUserLibraryManager>();
            tempDatabaseCleaner = new Mock<ITempDatabaseCleaner>();
            filesManager = new Mock<IFilesManager>();
            customInteriorManager = new Mock<ICustomInteriorManager>();
            httpContextReader = new Mock<IHttpContextReader>();

            database.Setup(d => d.CharacterRepository.GetAccountCharactersWithEstatesAndVehicles(It.IsAny<int>()))
                .ReturnsAsync(characters);
            database.Setup(d => d.BeginTransaction()).Returns(new DatabaseTransaction());
            database.Setup(d => d.EstateRepository.Update(It.IsNotNull<Estate>())).ReturnsAsync(true);
            database.Setup(d => d.UserRepository.FindById(It.IsAny<int>())).ReturnsAsync(user);
            database.Setup(d => d.UserRepository.Update(It.IsAny<User>())).ReturnsAsync(true);
            database.Setup(d => d.CharacterRepository.Find(It.IsAny<string>()))
                .ReturnsAsync(new Character());
            userService.Setup(us => us.GetUserWithCharacters(It.IsAny<int>())).ReturnsAsync(user);
            characterService.Setup(cs => cs.HasAnyCharacterEstate(It.IsAny<IEnumerable<Character>>(), It.IsAny<int>()))
                .ReturnsAsync(estate);
            purchaseService.Setup(ps => ps.CreatePurchase(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new Purchase());
            orderService.Setup(os => os.CreateOrder(It.IsAny<Order>())).ReturnsAsync(new Order());
            premiumUserLibraryManager.Setup(pm => pm.AddFileToLibrary(It.IsAny<IFormFile>(),
                    It.IsAny<PremiumFileType>(), It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(new PremiumFile());
            SetUpDefaultExecuteTransferCharacter();

            premiumAccountManager = new PremiumAccountManager(database.Object, characterService.Object,
                userService.Object, purchaseService.Object, adminActionService.Object, emailSender.Object,
                mtaManager.Object, orderService.Object, premiumUserLibraryManager.Object, tempDatabaseCleaner.Object,
                filesManager.Object, customInteriorManager.Object, httpContextReader.Object);
        }

        #region AddObjectProtection

        [Test]
        public async Task AddObjectProtection_AccountDoesNotHaveAnyCharacters_ReturnObjectProtectionResult()
        {
            database.Setup(d => d.CharacterRepository.GetAccountCharactersWithEstatesAndVehicles(It.IsAny<int>()))
                .ReturnsAsync(new List<Character>());

            var result = await premiumAccountManager.AddObjectProtection(addObjectProtectionRequest);

            Assert.That(result, Is.TypeOf<ObjectProtectionResult>());
            Assert.That(result.IsSucceeded, Is.False);
            Assert.That(result.ProtectionType, Is.EqualTo(addObjectProtectionRequest.ProtectionType));
            Assert.That(result.ObjectId, Is.EqualTo(addObjectProtectionRequest.ObjectId));
        }

        [Test]
        public async Task AddObjectProtection_CharactersDoesNotMatchObjectId_ReturnObjectProtectionResult()
        {
            database.Setup(d => d.CharacterRepository.GetAccountCharactersWithEstatesAndVehicles(It.IsAny<int>()))
                .ReturnsAsync(new List<Character>
                {
                    new TestCharacter(),
                });


            var result = await premiumAccountManager.AddObjectProtection(addObjectProtectionRequest);

            Assert.That(result, Is.TypeOf<ObjectProtectionResult>());
            Assert.That(result.IsSucceeded, Is.False);
            Assert.That(result.ProtectionType, Is.EqualTo(addObjectProtectionRequest.ProtectionType));
            Assert.That(result.ObjectId, Is.EqualTo(addObjectProtectionRequest.ObjectId));
        }

        [Test]
        public void AddObjectProtection_CreatingPurchaseFailed_ThrowPremiumOperationException()
        {
            purchaseService.Setup(ps => ps.CreatePurchase(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.AddObjectProtection(addObjectProtectionRequest),
                Throws.TypeOf<PremiumOperationException>());
        }

        [Test]
        public async Task AddObjectProtection_WhenCalled_ReturnObjectProtectionResult()
        {
            var result = await premiumAccountManager.AddObjectProtection(addObjectProtectionRequest);

            Assert.That(result, Is.TypeOf<ObjectProtectionResult>());
            Assert.That(result.IsSucceeded, Is.True);
            Assert.That(result.ProtectionType, Is.EqualTo(addObjectProtectionRequest.ProtectionType));
            Assert.That(result.ObjectId, Is.EqualTo(addObjectProtectionRequest.ObjectId));
        }

        #endregion

        #region AddSerialSlot

        [Test]
        public void AddSerialSlot_UserNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(It.IsAny<int>())).ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.AddSerialSlot(addSerialSlotRequest),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void AddSerialSlot_UpdatingUserFailed_ThrowPremiumOperationException()
        {
            database.Setup(d => d.UserRepository.Update(It.IsAny<User>())).ReturnsAsync(false);


            Assert.That(() => premiumAccountManager.AddSerialSlot(addSerialSlotRequest),
                Throws.TypeOf<PremiumOperationException>());
        }

        [Test]
        public void AddSerialSlot_CreatingPurchaseFailed_ThrowPremiumOperationException()
        {
            purchaseService.Setup(ps => ps.CreatePurchase(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.AddSerialSlot(addSerialSlotRequest),
                Throws.TypeOf<PremiumOperationException>());
        }

        [Test]
        public async Task AddSerialSlot_WhenCalled_ReturnAddSerialSlotResult()
        {
            var result = await premiumAccountManager.AddSerialSlot(addSerialSlotRequest);

            Assert.That(result, Is.TypeOf<AddSerialSlotResult>());
            Assert.That(result.IsSucceeded, Is.True);
        }

        #endregion

        #region AddCustomSkin

        [Test]
        public void AddCustomSkin_FileDoesNotExists_ThrowServerException()
        {
            Assert.That(() => premiumAccountManager.AddCustomSkin(null, 1, 1), Throws.TypeOf<ServerException>());
        }

        [Test]
        public void AddCustomSkin_CharacterDoesNotExists_ThrowDatabaseException()
        {
            database.Setup(d => d.CharacterRepository.Find(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.AddCustomSkin(file, 1, 1), Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public void AddCustomSkin_CreatingOrderFailed_ThrowDatabaseException()
        {
            orderService.Setup(os => os.CreateOrder(It.IsAny<Order>())).ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.AddCustomSkin(file, 1, 1), Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public void AddCustomSkin_AddingFileToLibrary_ThrowDatabaseException()
        {
            premiumUserLibraryManager.Setup(pm => pm.AddFileToLibrary(It.IsAny<IFormFile>(),
                    It.IsAny<PremiumFileType>(), It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.AddCustomSkin(file, 1, 1), Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public void AddCustomSkin_CreatingPurchaseFailed_ThrowPremiumOperationException()
        {
            purchaseService.Setup(ps => ps.CreatePurchase(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.AddCustomSkin(file, 1, 1), Throws.TypeOf<PremiumOperationException>());
        }

        [Test]
        public async Task AddCustomSkin_WhenCalled_ReturnTrue()
        {
            var result = await premiumAccountManager.AddCustomSkin(file, 1, 1);

            Assert.That(result, Is.True);
        }

        #endregion

        #region AddCustomInterior

        [Test]
        public void AddCustomInterior_FileDoesNotExists_ThrowServerException()
        {
            Assert.That(() => premiumAccountManager.AddCustomInterior(null, estate.Id),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public void AddCustomInterior_UserNotFound_ThrowEntityNotFoundException()
        {
            userService.Setup(us => us.GetUserWithCharacters(It.IsAny<int>())).ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.AddCustomInterior(file, estate.Id),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void AddCustomInterior_EstateDoesNotMatch_ThrowNoPermissionsException()
        {
            characterService.Setup(cs => cs.HasAnyCharacterEstate(It.IsAny<IEnumerable<Character>>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.AddCustomInterior(file, estate.Id),
                Throws.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void AddCustomInterior_WhenCalled_ClearGameTempObjectsAndInteriorsShouldBeCalled()
        {
            premiumAccountManager.AddCustomInterior(file, estate.Id);

            tempDatabaseCleaner.Verify(tc => tc.ClearGameTempObjectsAndInteriors(), Times.Once);
        }


        [Test]
        public void AddCustomInterior_AddingFileToLibraryFailed_ThrowDatabaseException()
        {
            premiumUserLibraryManager.Setup(pm => pm.AddFileToLibrary(It.IsAny<IFormFile>(),
                    It.IsAny<PremiumFileType>(), It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.AddCustomInterior(file, estate.Id),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public void AddCustomInterior_WhenCalled_ReplaceInFileShouldBeCalled()
        {
            premiumAccountManager.AddCustomInterior(file, estate.Id);

            filesManager.Verify(fm => fm.ReplaceInFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }

        [Test]
        public void AddCustomInterior_WhenCalled_InitGameTempObjectsAndInteriorsShouldBeCalled()
        {
            premiumAccountManager.AddCustomInterior(file, estate.Id);

            customInteriorManager.Verify(cm => cm.ExecuteAddCustomInterior(It.IsAny<PremiumFile>(),
                It.IsAny<IEnumerable<GameTempObject>>(), It.IsAny<GameTempInterior>()), Times.Once);
        }

        [Test]
        public void AddCustomInterior_CreatingOrderFailed_ThrowDatabaseException()
        {
            orderService.Setup(os => os.CreateOrder(It.IsAny<Order>())).ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.AddCustomInterior(file, estate.Id),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public void AddCustomInterior_WhenCalled_ExecuteAddCustomInteriorShouldBeCalled()
        {
            premiumAccountManager.AddCustomInterior(file, estate.Id);

            customInteriorManager.Verify(
                cm => cm.ExecuteAddCustomInterior(It.IsAny<PremiumFile>(), It.IsAny<IEnumerable<GameTempObject>>(),
                    It.IsAny<GameTempInterior>()), Times.Once);
        }
        
        [Test]
        public void AddCustomInterior_CreatingPurchaseFailed_ThrowPremiumOperationException()
        {
            purchaseService.Setup(ps => ps.CreatePurchase(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);
            
            Assert.That(() => premiumAccountManager.AddCustomInterior(file, estate.Id),
                Throws.TypeOf<PremiumOperationException>());
        }

        [Test]
        public async Task AddCustomInterior_WhenCalled_ReturnTrue()
        {
            var result = await premiumAccountManager.AddCustomInterior(file, estate.Id);

            Assert.That(result, Is.True);
        }

        #endregion

        #region TransferCharacter

        [Test]
        public void TransferCharacter_UserNotFound_ThrowEntityNotFoundException()
        {
            userService.Setup(us => us.GetUserWithCharacters(It.IsAny<int>())).ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.TransferCharacter(sourceCharacter.Id, targetCharacter.Id),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void TransferCharacter_SourceCharacterDoesNotExists_ThrowEntityNotFoundException()
        {
            Assert.That(() => premiumAccountManager.TransferCharacter(1, targetCharacter.Id),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void TransferCharacter_TargetCharacterDoesNotExists_ThrowEntityNotFoundException()
        {
            Assert.That(() => premiumAccountManager.TransferCharacter(sourceCharacter.Id, 1),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void TransferCharacter_TransferMoneyFailed_ThrowPremiumOperationException()
        {
            characterService.Setup(cs => cs.TransferMoney(It.IsAny<Character>(), It.IsAny<Character>()))
                .ReturnsAsync(false);

            Assert.That(() => premiumAccountManager.TransferCharacter(sourceCharacter.Id, targetCharacter.Id),
                Throws.TypeOf<PremiumOperationException>());
        }

        [Test]
        public void TransferCharacter_TransferEstatesAndVehiclesFailed_ThrowPremiumOperationException()
        {
            characterService.Setup(cs => cs.TransferEstatesAndVehicles(It.IsAny<IEnumerable<Estate>>(),
                    It.IsAny<IEnumerable<Vehicle>>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            Assert.That(() => premiumAccountManager.TransferCharacter(sourceCharacter.Id, targetCharacter.Id),
                Throws.TypeOf<PremiumOperationException>());
        }

        [Test]
        public void TransferCharacter_TransferGameItemsFailed_ThrowPremiumOperationException()
        {
            characterService.Setup(cs => cs.TransferGameItems(It.IsAny<Character>(), It.IsAny<Character>()))
                .ReturnsAsync(false);

            Assert.That(() => premiumAccountManager.TransferCharacter(sourceCharacter.Id, targetCharacter.Id),
                Throws.TypeOf<PremiumOperationException>());
        }

        [Test]
        public void TransferCharacter_InsertAdminActionFailed_ThrowPremiumOperationException()
        {
            adminActionService.Setup(a => a.InsertAdminAction(It.IsAny<AdminAction>()))
                .ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.TransferCharacter(sourceCharacter.Id, targetCharacter.Id),
                Throws.TypeOf<PremiumOperationException>());
        }

        [Test]
        public void TransferCharacter_CreatePurchaseFailed_ThrowPremiumOperationException()
        {
            purchaseService.Setup(ps => ps.CreatePurchase(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(() => null);

            Assert.That(() => premiumAccountManager.TransferCharacter(sourceCharacter.Id, targetCharacter.Id),
                Throws.TypeOf<PremiumOperationException>());
        }

        [Test]
        public void AddCustomInterior_WhenCalled_SendShouldBeCalled()
        {
            premiumAccountManager.TransferCharacter(sourceCharacter.Id, targetCharacter.Id);

            emailSender.Verify(es => es.Send(It.IsAny<EmailMessage>()), Times.Once);
        }

        [Test]
        public void AddCustomInterior_WhenCalled_CallFunctionShouldBeCalled()
        {
            premiumAccountManager.TransferCharacter(sourceCharacter.Id, targetCharacter.Id);

            mtaManager.Verify(mm => mm.CallFunction(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MtaLuaArgs>()),
                Times.Once);
        }

        [Test]
        public async Task AddCustomInterior_WhenCalled_CallFunctionShouldBeCalled1()
        {
            mtaManager.Setup(mm => mm.CallFunction(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MtaLuaArgs>()))
                .Throws<MTARequestException>();

            var result = await premiumAccountManager.TransferCharacter(sourceCharacter.Id, targetCharacter.Id);
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task TransferCharacter_WhenCalled_ReturnTrue()
        {
            var result = await premiumAccountManager.TransferCharacter(sourceCharacter.Id, targetCharacter.Id);

            Assert.That(result, Is.True);
        }

        #endregion

        #region private

        public void SetUpDefaultExecuteTransferCharacter()
        {
            characterService.Setup(cs => cs.TransferMoney(It.IsAny<Character>(), It.IsAny<Character>()))
                .ReturnsAsync(true);
            characterService.Setup(cs => cs.TransferEstatesAndVehicles(It.IsAny<IEnumerable<Estate>>(),
                    It.IsAny<IEnumerable<Vehicle>>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            characterService.Setup(cs => cs.TransferGameItems(It.IsAny<Character>(), It.IsAny<Character>()))
                .ReturnsAsync(true);
            adminActionService.Setup(a => a.InsertAdminAction(It.IsAny<AdminAction>()))
                .ReturnsAsync(new AdminAction());
        }

        #endregion
    }
}