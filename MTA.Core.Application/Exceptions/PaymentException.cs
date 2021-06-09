using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class PaymentException : ApplicationException
    {
        public string ErrorCode { get; }

        public PaymentException(string message, string errorCode = ErrorCodes.PaymentError) : base(message)
            => (ErrorCode) = (errorCode);
    }
}