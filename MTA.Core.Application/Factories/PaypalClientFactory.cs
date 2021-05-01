using MTA.Core.Application.Factories.Interfaces;
using MTA.Core.Application.Settings;
using PayPalCheckoutSdk.Core;

namespace MTA.Core.Application.Factories
{
    public class PaypalClientFactory : IPaypalClientFactory
    {
        public PayPalHttpClient CreateClient(PaypalSettings paypalSettings)
        {
            PayPalEnvironment environment = paypalSettings.Mode == "sandbox"
                ? new SandboxEnvironment(paypalSettings.ClientId, paypalSettings.ClientSecret)
                : new LiveEnvironment(paypalSettings.ClientId, paypalSettings.ClientSecret);

            return new PayPalHttpClient(environment);
        }
    }
}