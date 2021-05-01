using MTA.Core.Application.Settings;

namespace MTA.UnitTests.TestModels
{
    public class TestPaypalSettings : PaypalSettings
    {
        public TestPaypalSettings()
        {
            Mode = "sandbox";
            ClientId = "sandbox";
            ClientSecret = "sandbox";
            ReturnUrl = "sandbox";
            CancelUrl = "sandbox";
        }
    }
}