using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Persistence.Database;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class UserManagerTests
    {
        private BlockAccountRequest request;
        private User user;

        private Mock<IDatabase> database;
        private Mock<IBanService> banService;
        private Mock<IReadOnlySerialService> serialService;
        private Mock<IRolesService> roleService;
        private Mock<IHttpContextReader> httpContextReader;
        private UserManager userManager;

        [SetUp]
        public void SetUp()
        {
            user = new User();
            request = new BlockAccountRequest
            {
                AccountId = 1,
                Reason = It.IsNotNull<string>(),
                Serials = new List<string> {"1", "2"},
                Ips = new List<string> {"1", "2"}
            };

            var gameItems = new List<GameItem> {new GameItem()};

            database = new Mock<IDatabase>();
            banService = new Mock<IBanService>();
            serialService = new Mock<IReadOnlySerialService>();
            roleService = new Mock<IRolesService>();
            httpContextReader = new Mock<IHttpContextReader>();

            database.Setup(d => d.BeginTransaction()).Returns(new DatabaseTransaction());
            database.Setup(d => d.UserRepository.FindById(It.IsAny<int>())).ReturnsAsync(user);
            database.Setup(d => d.UserRepository.Update(It.IsAny<User>())).ReturnsAsync(true);
            database.Setup(d => d.UserRepository.GetUserWithCharacters(It.IsNotNull<int>())).ReturnsAsync(user);
            database.Setup(d => d.GameItemRepository.GetAccountItems(It.IsNotNull<IEnumerable<int>>()))
                .ReturnsAsync(gameItems);
            database.Setup(d => d.CharacterRepository.DeleteRange(It.IsNotNull<IEnumerable<Character>>()))
                .ReturnsAsync(true);
            database.Setup(d => d.EstateRepository.DeleteRange(It.IsNotNull<IEnumerable<Estate>>())).ReturnsAsync(true);
            database.Setup(d => d.VehicleRepository.DeleteRange(It.IsNotNull<IEnumerable<Vehicle>>()))
                .ReturnsAsync(true);
            database.Setup(d => d.GameItemRepository.DeleteRange(It.IsNotNull<IEnumerable<GameItem>>()))
                .ReturnsAsync(true);

            banService.Setup(bs => bs.AddBan(It.IsNotNull<Ban>())).ReturnsAsync(new Ban());

            serialService.Setup(ss => ss.SerialExists(It.IsNotNull<string>(), It.IsNotNull<int>()))
                .ReturnsAsync(true);

            roleService.Setup(rs => rs.IsPermitted(It.IsNotNull<int>(), Constants.AllOwnersRoles))
                .ReturnsAsync(true);

            userManager = new UserManager(database.Object, banService.Object, serialService.Object, roleService.Object,
                httpContextReader.Object);
        }

        #region BlockAccount

        [Test]
        public void BlockAccount_NotPermitted_ThrowNoPermissionsException()
        {
            roleService.Setup(rs => rs.IsPermitted(It.IsNotNull<int>(), Constants.AllOwnersRoles))
                .ReturnsAsync(false);

            Assert.That(() => userManager.BlockAccount(request), Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void BlockAccount_SomeSerialDoesNotExist_ThrowServerException()
        {
            serialService.Setup(ss => ss.SerialExists(It.IsNotNull<string>(), It.IsNotNull<int>()))
                .ReturnsAsync(false);

            Assert.That(() => userManager.BlockAccount(request), Throws.Exception.TypeOf<ServerException>());
        }

        [Test]
        public void BlockAccount_AddingSomeBansFailed_ThrowDatabaseException()
        {
            banService.Setup(bs => bs.AddBan(It.IsNotNull<Ban>())).ReturnsAsync(() => null);

            Assert.That(() => userManager.BlockAccount(request), Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task BlockAccount_WhenCalled_ReturnTrue()
        {
            var result = await userManager.BlockAccount(request);

            Assert.That(result, Is.True);
        }

        #endregion

        #region AddCredits

        [Test]
        public void AddCredits_EmptyUser_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(It.IsAny<int>())).ReturnsAsync(() => null);

            Assert.That(() => userManager.AddCredits(It.IsAny<int>(), It.IsAny<int>()),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void AddCredits_UpdatingUserFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.UserRepository.Update(It.IsAny<User>())).ReturnsAsync(false);

            Assert.That(() => userManager.AddCredits(It.IsAny<int>(), It.IsAny<int>()),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task AddCredits_WhenCalled_ReturnAddCreditsResult()
        {
            var result = await userManager.AddCredits(It.IsAny<int>(), It.IsAny<int>());

            Assert.That(result, Is.TypeOf<AddCreditsResult>().And.Not.Null);
        }

        #endregion

        #region CleanAccount

        [Test]
        public void CleanAccount_UserNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.GetUserWithCharacters(It.IsNotNull<int>())).ReturnsAsync(() => null);

            Assert.That(() => userManager.CleanAccount(0), Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void CleanAccount_DeletingCharactersFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.CharacterRepository.DeleteRange(It.IsNotNull<IEnumerable<Character>>()))
                .ReturnsAsync(false);

            Assert.That(() => userManager.CleanAccount(0), Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void CleanAccount_DeletingEstatesFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.EstateRepository.DeleteRange(It.IsNotNull<IEnumerable<Estate>>()))
                .ReturnsAsync(false);

            Assert.That(() => userManager.CleanAccount(0), Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void CleanAccount_DeletingVehiclesFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.VehicleRepository.DeleteRange(It.IsNotNull<IEnumerable<Vehicle>>()))
                .ReturnsAsync(false);

            Assert.That(() => userManager.CleanAccount(0), Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void CleanAccount_DeletingGameItemsFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.GameItemRepository.DeleteRange(It.IsNotNull<IEnumerable<GameItem>>()))
                .ReturnsAsync(false);

            Assert.That(() => userManager.CleanAccount(0), Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task CleanAccount_WhenCalled_ReturnTrue()
        {
            var result = await userManager.CleanAccount(0);

            Assert.That(result, Is.True);
        }

        #endregion
    }
}