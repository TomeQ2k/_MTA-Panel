using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;
using MTA.Core.Common.Enums;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class SetOrderApprovalStateCommandTests
    {
        private SetOrderApprovalStateCommand setOrderApprovalStateCommand;

        private Mock<IOrderService> orderService;

        private SetOrderApprovalStateRequest request;

        [SetUp]
        public void SetUp()
        {
            request = new SetOrderApprovalStateRequest {OrderId = "id", ApprovalState = StateType.Accept};

            orderService = new Mock<IOrderService>();
            var httpContextReader = new Mock<IHttpContextReader>();
            var notifier = new Mock<INotifier>();
            var hubManager = new Mock<IHubManager<NotifierHub>>();
            var mapper = new Mock<IMapper>();

            orderService.Setup(o =>
                    o.SetOrderApprovalState(It.IsNotNull<string>(), It.IsAny<StateType>(), It.IsAny<string>()))
                .ReturnsAsync(new SetApprovalStateResult(true, 1));

            setOrderApprovalStateCommand =
                new SetOrderApprovalStateCommand(orderService.Object, httpContextReader.Object, notifier.Object,
                    hubManager.Object, mapper.Object);
        }

        [Test]
        public void Handle_SettingApprovalStateOfOrderFailed_ThrowPremiumOperationException()
        {
            orderService.Setup(o =>
                    o.SetOrderApprovalState(It.IsNotNull<string>(), It.IsAny<StateType>(), It.IsAny<string>()))
                .ReturnsAsync(new SetApprovalStateResult(false, 1));

            Assert.That(() => setOrderApprovalStateCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<PremiumOperationException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnSetOrderApprovalStateResponse()
        {
            var result = await setOrderApprovalStateCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null
                .And.TypeOf<SetOrderApprovalStateResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}