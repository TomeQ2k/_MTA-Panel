using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class CannotGenerateTokenException : ApplicationException
    {
        public string ErrorCode { get; }

        public CannotGenerateTokenException(string message, string errorCode = ErrorCodes.CannotGenerateToken) : base(message)
            => (ErrorCode) = (errorCode);
    }
}