using MTA.Core.Application.Settings;
using PayPalCheckoutSdk.Core;

namespace MTA.Core.Application.Factories.Interfaces
{
    public interface IPaypalClientFactory
    {
        PayPalHttpClient CreateClient(PaypalSettings paypalSettings);
    }
}