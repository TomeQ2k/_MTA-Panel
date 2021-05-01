using System;
using MTA.Core.Application.Helpers;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class CaptchaException : Exception
    {
        public string ErrorCode { get; }

        public CaptchaException(string message = ErrorMessages.CaptchaInvalidErrorMessage,
            string errorCode = ErrorCodes.CaptchaError) : base(message)
            => (ErrorCode) = (errorCode);
    }
}