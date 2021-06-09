using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class InvalidCredentialsException : ApplicationException
    {
        public string ErrorCode { get; }

        public InvalidCredentialsException(string message, string errorCode = ErrorCodes.InvalidCredentials) : base(message)
            => (ErrorCode) = (errorCode);
    }
}