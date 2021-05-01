using System.Threading.Tasks;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;

namespace MTA.Core.Application.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> CreatePayment(params PaymentUnit[] paymentUnits);
        Task<PaymentResult> CapturePayment(string orderId);
    }
}