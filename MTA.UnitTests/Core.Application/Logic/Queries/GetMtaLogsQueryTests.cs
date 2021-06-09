using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Queries;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Logging;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Queries
{
    [TestFixture]
    public class GetMtaLogsQueryTests
    {
        private Mock<ILogReader> logReader;
        private Mock<IRolesService> rolesService;
        private Mock<IReadOnlyCharacterService> characterService;
        private Mock<IReadOnlyUserService> userService;
        private Mock<IHttpContextReader> httpContextReader;
        private GetMtaLogsQuery getMtaLogsQuery;
        private GetMtaLogsRequest request;

        [SetUp]
        public void SetUp()
        {
            var users = new List<User>
            {
                new User(),
                new User()
            };
            var characters = new List<Character>
            {
                new Character(),
                new Character()
            };
            var logs = new List<MtaLogModel>
            {
                new MtaLogModel()
            };

            request = new GetMtaLogsRequest
            {
                MinTimeAgo = TimeAgoType.Hour,
                MaxTimeAgo = TimeAgoType.Day,
                Actions = new[] {LogAction.Anticheat},
                ContentFilterType = ContentFilterType.All,
                SourceAffectedFilterType = SourceAffectedFilterType.All,
                SortType = DateSortType.Ascending
            };

            logReader = new Mock<ILogReader>();
            rolesService = new Mock<IRolesService>();
            characterService = new Mock<IReadOnlyCharacterService>();
            userService = new Mock<IReadOnlyUserService>();
            httpContextReader = new Mock<IHttpContextReader>();

            rolesService.Setup(rs => rs.IsPermitted(It.IsAny<int>(), It.IsAny<RoleType[]>()))
                .ReturnsAsync(true);
            userService.Setup(us => us.GetUsersByUsername(It.IsAny<string>()))
                .ReturnsAsync(users);
            characterService.Setup(cs => cs.GetCharactersByCharactername(It.IsAny<string>()))
                .ReturnsAsync(characters);
            logReader.Setup(lr =>
                    lr.GetMtaLogsFromDatabase(It.IsAny<GetMtaLogsRequest>(),
                        It.IsAny<IEnumerable<SourceAffectedModel>>()))
                .ReturnsAsync(logs);

            getMtaLogsQuery = new GetMtaLogsQuery(logReader.Object, rolesService.Object, characterService.Object,
                userService.Object, httpContextReader.Object);
        }

        [Test]
        public void Handle_InvalidActionsAndSourceAffcetedLogType_ThrowNoPermissionsException()
        {
            Assert.That(() => getMtaLogsQuery.Handle(request with{Actions = new[] {LogAction.PhoneSms}}, new CancellationToken()),
                Throws.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void Handle_UserWithoutPermission_ThrowNoPermissionsException()
        {
            rolesService.Setup(rs => rs.IsPermitted(It.IsAny<int>(), It.IsAny<RoleType[]>()))
                .ReturnsAsync(false);

            Assert.That(() => getMtaLogsQuery.Handle(request, new CancellationToken()),
                Throws.TypeOf<NoPermissionsException>());
        }

        [Test]
        public async Task Handle_WhenCalled_VerifyGetCharactersByName()
        {
            await getMtaLogsQuery.Handle(request with{SourceAffectedLogType = SourceAffectedLogType.Character}, new CancellationToken());
            characterService.Verify(cs => cs.GetCharactersByCharactername(It.IsAny<string>()));
        }

        [Test]
        public async Task Handle_WhenCalled_VerifyGetUsersByUsername()
        {
            await getMtaLogsQuery.Handle(request with {SourceAffectedLogType = SourceAffectedLogType.Account},
                new CancellationToken());
            userService.Verify(cs => cs.GetUsersByUsername(It.IsAny<string>()));
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnGetMtaLogsResponse()
        {
            var result = await getMtaLogsQuery.Handle(request, new CancellationToken());

            Assert.That(result, Is.Not.Null.And.TypeOf<GetMtaLogsResponse>());
        }
    }
}