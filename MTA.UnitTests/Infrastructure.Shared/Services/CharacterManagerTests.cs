using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class CharacterManagerTests
    {
        private CharacterManager characterManager;

        private Mock<IDatabase> database;
        private Mock<IRolesService> rolesService;
        private Mock<IHttpContextReader> httpContextReader;

        private Character character;

        [SetUp]
        public void SetUp()
        {
            database = new Mock<IDatabase>();
            rolesService = new Mock<IRolesService>();
            httpContextReader = new Mock<IHttpContextReader>();

            character = new Character();

            httpContextReader.Setup(h => h.CurrentUserId).Returns(It.IsNotNull<int>());

            rolesService.Setup(rs => rs.IsPermitted(It.IsNotNull<int>(), Constants.AdminRoles))
                .ReturnsAsync(true);

            database.Setup(d => d.CharacterRepository.FindById(It.IsNotNull<int>()))
                .ReturnsAsync(character);
            database.Setup(d => d.CharacterRepository.Update(character))
                .ReturnsAsync(true);

            characterManager = new CharacterManager(database.Object, rolesService.Object, httpContextReader.Object);
        }

        #region ToggleBlockCharacter

        [Test]
        public void ToggleBlockCharacter_NotPermitted_ThrowNoPermissionsException()
        {
            rolesService.Setup(rs => rs.IsPermitted(It.IsNotNull<int>(), Constants.AdminRoles))
                .ReturnsAsync(false);

            Assert.That(() => characterManager.ToggleBlockCharacter(It.IsAny<int>()),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void ToggleBlockCharacter_PermittedButCharacterNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.CharacterRepository.FindById(It.IsNotNull<int>()))
                .ReturnsAsync(() => null);

            Assert.That(() => characterManager.ToggleBlockCharacter(It.IsAny<int>()),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public async Task ToggleBlockCharacter_WhenCalled_ReturnBlockCharacterResult()
        {
            var result = await characterManager.ToggleBlockCharacter(It.IsAny<int>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BlockCharacterResult>());
            Assert.That(result.IsSucceeded, Is.True);
        }

        [Test]
        public void ToggleBlockCharacter_UpdatingCharacterFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.CharacterRepository.Update(character))
                .ReturnsAsync(false);

            Assert.That(() => characterManager.ToggleBlockCharacter(It.IsAny<int>()),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        #endregion
    }
}