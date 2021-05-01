using System.Globalization;
using PayPalCheckoutSdk.Orders;

namespace MTA.Core.Application.Models
{
    public record PaymentUnit
    {
        public AmountWithBreakdown Amount { get; init; }

        public static PaymentUnit Create(decimal price) => new PaymentUnit
        {
            Amount = new AmountWithBreakdown
            {
                CurrencyCode = "PLN",
                Value = price.ToString(CultureInfo.InvariantCulture)
            }
        };
    }
}