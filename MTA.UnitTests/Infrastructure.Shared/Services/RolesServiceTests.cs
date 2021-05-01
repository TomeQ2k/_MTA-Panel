using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Results;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using MTA.UnitTests.TestModels;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class RolesServiceTests
    {
        private RolesService rolesService;

        private Mock<IDatabase> database;

        private User user;

        private const int DevId = -1;

        [SetUp]
        public void SetUp()
        {
            user = new User();
            var inMemorySettings = new Dictionary<string, string>()
            {
                {AppSettingsKeys.DevId, $"{DevId}"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            database = new Mock<IDatabase>();

            rolesService = new RolesService(database.Object, configuration);
        }

        [Test]
        public void AdmitRole_UserIsNull_ThrowEntityNotFoundException()
        {
            Assert.That(() => rolesService.AdmitRole(null, RoleType.Admin),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void RevokeRole_UserIsNull_ThrowEntityNotFoundException()
        {
            Assert.That(() => rolesService.RevokeRole(null, RoleType.Admin),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        #region IsPermitted

        [Test]
        public void IsPermitted_UserIsNull_ThrowEntityNotFoundException()
        {
            Assert.That(() => rolesService.IsPermitted(null, RoleType.Admin),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void IsPermitted_SelectQueryFirstFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.SelectQueryFirst<IsPermittedResult>(It.IsAny<SqlQuery>()))
                .ReturnsAsync(() => null);

            Assert.That(() => rolesService.IsPermitted(user, RoleType.Admin),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task IsPermitted_UserIsDev_ReturnTrue()
        {
            user = new DevUser();

            var result = await rolesService.IsPermitted(DevId, Constants.AdminRoles);

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task IsPermitted_IsPermittedResultFetchedFromDatabase_ReturnIsPermitted(bool isPermitted)
        {
            var isPermittedResult = new IsPermittedResult() {IsPermitted = isPermitted};

            database.Setup(d => d.SelectQueryFirst<IsPermittedResult>(It.IsAny<SqlQuery>()))
                .ReturnsAsync(() => isPermittedResult);

            var result = await rolesService.IsPermitted(It.IsAny<int>(), Constants.AdminRoles);

            Assert.That(result, Is.EqualTo(isPermittedResult.IsPermitted));
        }

        #endregion
    }
}