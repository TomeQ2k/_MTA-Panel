using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using MTA.UnitTests.TestModels;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class ReportValidationServiceTests
    {
        private ReportValidationService reportValidationService;

        private Mock<IDatabase> database;
        private Mock<IRolesService> rolesService;
        private Mock<IHttpContextReader> httpContextReader;
        private Mock<IList<IReportMember>> reportMembers;

        private Report report;
        private User user;
        private IList<ReportImage> reportImages;
        private IList<IFormFile> images;

        private const int UserId = 1;
        private const int DevId = -1;

        [SetUp]
        public void SetUp()
        {
            report = new Report();
            user = new User();
            reportImages = new List<ReportImage>()
            {
                new ReportImage()
            };
            images = new List<IFormFile>()
            {
                new FormFile(It.IsNotNull<Stream>(), It.IsNotNull<long>(),
                    It.IsNotNull<long>(),
                    It.IsNotNull<string>(), It.IsNotNull<string>())
            };
            var inMemorySettings = new Dictionary<string, string>()
            {
                {AppSettingsKeys.DevId, $"{DevId}"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            database = new Mock<IDatabase>();
            rolesService = new Mock<IRolesService>();
            httpContextReader = new Mock<IHttpContextReader>();
            reportMembers = new Mock<IList<IReportMember>>();

            database.Setup(d => d.UserRepository.FindById(UserId)).ReturnsAsync(user);
            rolesService.Setup(r => r.IsPermitted(UserId, Constants.AllOwnersRoles))
                .ReturnsAsync(true);
            httpContextReader.Setup(h => h.CurrentUserId).Returns(UserId);

            reportValidationService = new ReportValidationService(database.Object, rolesService.Object,
                httpContextReader.Object, reportMembers.Object, configuration);
        }

        #region ValidatePermissions

        [Test]
        public async Task ValidatePermissions_UserIsDev_ReturnTrue()
        {
            user = new DevUser();

            rolesService.Setup(r => r.IsPermitted(DevId, Constants.AllOwnersRoles))
                .ReturnsAsync(false);

            var result =
                await reportValidationService.ValidatePermissions(DevId, report, ReportPermission.AddComment);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task ValidatePermissions_UserIsOwnerOrViceOwner_ReturnTrue()
        {
            var result =
                await reportValidationService.ValidatePermissions(UserId, report, ReportPermission.AddComment);

            Assert.That(result, Is.True);
        }

        #endregion

        #region IsUserReportMember

        [Test]
        public async Task IsUserReportMember_UserIsOwnerOrViceOwner_ReturnTrue()
        {
            var result =
                await reportValidationService.IsUserReportMember(UserId, report);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsUserReportMember_UserIsCreator_ReturnTrue()
        {
            rolesService.Setup(r => r.IsPermitted(UserId, Constants.AllOwnersRoles))
                .ReturnsAsync(false);

            report.CreatedBy(UserId);

            var result =
                await reportValidationService.IsUserReportMember(UserId, report);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsUserReportMember_UserIsAssignee_ReturnTrue()
        {
            rolesService.Setup(r => r.IsPermitted(UserId, Constants.AllOwnersRoles))
                .ReturnsAsync(false);

            report.AssigneTo(UserId);

            var result =
                await reportValidationService.IsUserReportMember(UserId, report);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsUserReportMember_UserIsSubscriber_ReturnTrue()
        {
            rolesService.Setup(r => r.IsPermitted(UserId, Constants.AllOwnersRoles))
                .ReturnsAsync(false);

            report.ReportSubscribers.Add(ReportSubscriber.Create(It.IsNotNull<string>(), UserId));

            var result =
                await reportValidationService.IsUserReportMember(UserId, report);

            Assert.That(result, Is.True);
        }

        [Test]
        public async Task IsUserReportMember_UserIsNotMemberOfReport_ReturnFalse()
        {
            rolesService.Setup(r => r.IsPermitted(UserId, Constants.AllOwnersRoles))
                .ReturnsAsync(false);

            var result =
                await reportValidationService.IsUserReportMember(UserId, report);

            Assert.That(result, Is.False);
        }

        #endregion

        #region ValidateMaxFilesCountAndSizePerUser

        [Test]
        public void ValidateMaxFilesCountAndSizePerUser_ReportNotFound_ThrowEntityNotFoundException()
        {
            report = null;

            Assert.That(() => reportValidationService.ValidateMaxFilesCountAndSizePerUser(UserId, report, images),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void ValidateMaxFilesCountAndSizePerUser_UserNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(UserId)).ReturnsAsync(() => null);

            Assert.That(() => reportValidationService.ValidateMaxFilesCountAndSizePerUser(UserId, report, images),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        #endregion

        #region FindCategoriesToRead

        [Test]
        public void FindCategoriesToRead_UserNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(UserId)).ReturnsAsync(() => null);

            Assert.That(() => reportValidationService.FindCategoriesToRead(),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public async Task FindCategoriesToRead_UserIsAdmin_ReturnFindCategoriesToReadResult()
        {
            user.SetRoles(admin: 2);

            var result = await reportValidationService.FindCategoriesToRead();

            Assert.That(result, Is.Not.Null
                .And
                .TypeOf<FindCategoriesToReadResult>());
        }

        [Test]
        public async Task FindCategoriesToRead_UserIsSupporter_ReturnFindCategoriesToReadResult()
        {
            user.SetRoles(supporter: 1);

            var result = await reportValidationService.FindCategoriesToRead();

            Assert.That(result, Is.Not.Null
                .And
                .TypeOf<FindCategoriesToReadResult>());
        }

        [Test]
        public void FindCategoriesToRead_UserIsNotAdminAndSupporter_ThrowNoPermissionsException()
        {
            Assert.That(() => reportValidationService.FindCategoriesToRead(),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        #endregion
    }
}