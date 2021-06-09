using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class AuthException : ApplicationException
    {
        public string ErrorCode { get; }

        public AuthException(string message, string errorCode = ErrorCodes.AuthError) : base(message)
            => (ErrorCode) = (errorCode);
    }
}