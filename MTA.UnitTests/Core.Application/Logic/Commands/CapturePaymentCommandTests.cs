using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class CapturePaymentCommandTests
    {
        private CapturePaymentRequest request;

        private Mock<IPaymentService> paymentService;

        private CapturePaymentCommand capturePaymentCommand;

        [SetUp]
        public void SetUp()
        {
            request = new CapturePaymentRequest
            {
                Token = "token"
            };

            paymentService = new Mock<IPaymentService>();
            var httpContextReader = new Mock<IHttpContextReader>();
            
            paymentService.Setup(ps => ps.CapturePayment(It.IsAny<string>()))
                .ReturnsAsync(new PaymentResult
                {
                    StatusCode = HttpStatusCode.Created
                });

            capturePaymentCommand = new CapturePaymentCommand(paymentService.Object, httpContextReader.Object);
        }

        [Test]
        public void Handle_CapturingPaymentFailed_ThrowPaymentException()
        {
            paymentService.Setup(ps => ps.CapturePayment(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            Assert.That(() => capturePaymentCommand.Handle(request, CancellationToken.None),
                Throws.TypeOf<PaymentException>());
        }

        [Test]
        public void Handle_CapturingPaymentNotPass_ThrowPaymentException()
        {
            paymentService.Setup(ps => ps.CapturePayment(It.IsAny<string>()))
                .ReturnsAsync(new PaymentResult
                {
                    StatusCode = HttpStatusCode.Accepted
                });

            Assert.That(() => capturePaymentCommand.Handle(request, CancellationToken.None),
                Throws.TypeOf<PaymentException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnCapturePaymentResponse()
        {
            var result = await capturePaymentCommand.Handle(request, CancellationToken.None);

            Assert.That(result, Is.TypeOf<CapturePaymentResponse>().And.Not.Null);
            Assert.That(result.IsVerified, Is.True);
        }
    }
}