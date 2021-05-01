using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Logic.Handlers.Queries;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Queries
{
    [TestFixture]
    public class GetUserBansQueryTests
    {
        private GetUserBansQuery getUserBansQuery;

        private Mock<IReadOnlyBanService> banService;
        private Mock<IReadOnlyAdminActionService> adminActionService;
        private Mock<IMapper> mapper;

        private GetUserBansRequest request;
        private List<AdminAction> penalties;
        private List<Ban> bans;

        [SetUp]
        public void SetUp()
        {
            request = new GetUserBansRequest();

            penalties = new List<AdminAction>
            {
                new AdminAction(), new AdminAction()
            };

            bans = new List<Ban>()
            {
                new Ban(), new Ban()
            };

            var penaltyDtos = new List<PenaltyDto>
            {
                new PenaltyDto(), new PenaltyDto()
            };

            banService = new Mock<IReadOnlyBanService>();
            adminActionService = new Mock<IReadOnlyAdminActionService>();
            mapper = new Mock<IMapper>();

            banService.Setup(bs => bs.GetUserBans()).ReturnsAsync(bans);
            adminActionService.Setup(aas => aas.GetAdminActionsAsUserBans()).ReturnsAsync(penalties);
            mapper.Setup(m => m.Map<IEnumerable<PenaltyDto>>(penalties)).Returns(penaltyDtos);
            mapper.Setup(m => m.Map<IEnumerable<PenaltyDto>>(bans)).Returns(penaltyDtos);

            getUserBansQuery = new GetUserBansQuery(banService.Object, adminActionService.Object, mapper.Object);
        }

        [Test]
        public async Task Handle_AnyPenaltiesFound_ReturnResponseWithBansFound()
        {
            adminActionService.Setup(aas => aas.GetAdminActionsAsUserBans()).ReturnsAsync(new List<AdminAction>());

            var result = await getUserBansQuery.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSucceeded, Is.True);
            Assert.That(result.Bans, Is.InstanceOf<IEnumerable<PenaltyDto>>());
            Assert.That(result.Bans.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Handle_AnyPenaltiesFound_VerifyGetUserBansCalled()
        {
            adminActionService.Setup(aas => aas.GetAdminActionsAsUserBans()).ReturnsAsync(new List<AdminAction>());

            getUserBansQuery.Handle(request, It.IsNotNull<CancellationToken>());

            banService.Verify(bs => bs.GetUserBans());
        }

        [Test]
        public void Handle_AnyPenaltiesFound_VerifyMapBansCalled()
        {
            adminActionService.Setup(aas => aas.GetAdminActionsAsUserBans()).ReturnsAsync(new List<AdminAction>());

            getUserBansQuery.Handle(request, It.IsNotNull<CancellationToken>());

            mapper.Verify(m => m.Map<IEnumerable<PenaltyDto>>(bans));
            mapper.Verify(m => m.Map<IEnumerable<PenaltyDto>>(penalties), Times.Never);
        }

        [Test]
        public async Task Handle_SomePenaltiesFound_ReturnResponseWithPenaltiesFound()
        {
            var result = await getUserBansQuery.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSucceeded, Is.True);
            Assert.That(result.Bans, Is.InstanceOf<IEnumerable<PenaltyDto>>());
            Assert.That(result.Bans.Count(), Is.EqualTo(2));
        }

        [Test]
        public void Handle_SomePenaltiesFound_VerifyMapPenaltiesCalled()
        {
            getUserBansQuery.Handle(request, It.IsNotNull<CancellationToken>());

            mapper.Verify(m => m.Map<IEnumerable<PenaltyDto>>(penalties));
            mapper.Verify(m => m.Map<IEnumerable<PenaltyDto>>(bans), Times.Never);
        }
    }
}