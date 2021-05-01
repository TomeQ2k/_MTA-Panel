using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class PremiumOperationException : Exception
    {
        public string ErrorCode { get; }

        public PremiumOperationException(string message, string errorCode = ErrorCodes.PremiumError) : base(message)
            => (ErrorCode) = (errorCode);
    }
}