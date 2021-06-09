using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class CreatePaymentCommandTests
    {
        private Mock<IPaymentService> paymentService;

        private CreatePaymentCommand createPaymentCommand;

        [SetUp]
        public void SetUp()
        {
            paymentService = new Mock<IPaymentService>();
            var httpContextReader = new Mock<IHttpContextReader>();

            paymentService.Setup(ps => ps.CreatePayment(It.IsAny<PaymentUnit[]>()))
                .ReturnsAsync(new PaymentResult());

            createPaymentCommand = new CreatePaymentCommand(paymentService.Object, httpContextReader.Object);
        }

        [Test]
        public void Handle_CreatingPaymentFailed_ThrowPaymentException()
        {
            paymentService.Setup(ps => ps.CreatePayment(It.IsAny<PaymentUnit[]>()))
                .ReturnsAsync(() => null);

            Assert.That(() => createPaymentCommand.Handle(new CreatePaymentRequest(), CancellationToken.None),
                Throws.TypeOf<PaymentException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnCreatePaymentResponse()
        {
            var result = await createPaymentCommand.Handle(new CreatePaymentRequest(), CancellationToken.None);

            Assert.That(result, Is.TypeOf<CreatePaymentResponse>().And.Not.Null);
        }
    }
}