using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class TokenExpiredException : ApplicationException
    {
        public string ErrorCode { get; }

        public TokenExpiredException(string message = ErrorMessages.TokenExpiredErrorMessage,
            string errorCode = ErrorCodes.TokenExpired) : base(message)
            => (ErrorCode) = (errorCode);
    }
}