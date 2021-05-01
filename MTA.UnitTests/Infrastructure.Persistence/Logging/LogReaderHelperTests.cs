using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logging;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Persistence.Logging;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Persistence.Logging
{
    [TestFixture]
    public class LogReaderHelperTests
    {
        private LogReaderHelper logReaderHelper;

        private Mock<IDatabase> database;
        private Mock<IHttpContextReader> httpContextReader;
        private Mock<LogActionPermissionDictionary> logActionPermissionDictionary;

        private User user;

        private const int UserId = 1;
        private const int DevId = -1;

        [SetUp]
        public void SetUp()
        {
            user = new User();

            database = new Mock<IDatabase>();
            httpContextReader = new Mock<IHttpContextReader>();
            logActionPermissionDictionary = new Mock<LogActionPermissionDictionary>();
            var inMemorySettings = new Dictionary<string, string>()
            {
                {AppSettingsKeys.DevId, $"{DevId}"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            database.Setup(d => d.UserRepository.FindUserById(UserId)).ReturnsAsync(user);
            httpContextReader.Setup(h => h.CurrentUserId).Returns(UserId);

            logReaderHelper = new LogReaderHelper(database.Object, httpContextReader.Object,
                logActionPermissionDictionary.Object, configuration);
        }

        #region GetAllowedLogActions

        [Test]
        public void GetAllowedLogActions_UserNotAuthorized_ThrowAuthException()
        {
            database.Setup(d => d.UserRepository.FindUserById(UserId)).ReturnsAsync(() => null);

            Assert.That(() => logReaderHelper.GetAllowedLogActions(), Throws.Exception.TypeOf<AuthException>());
        }

        #endregion
    }
}