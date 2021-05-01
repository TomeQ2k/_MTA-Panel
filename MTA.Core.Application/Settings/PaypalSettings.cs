namespace MTA.Core.Application.Settings
{
    public class PaypalSettings
    {
        public string Mode { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}