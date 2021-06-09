using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class OldPasswordInvalidException : ApplicationException
    {
        public string ErrorCode { get; }

        public OldPasswordInvalidException(string message, string errorCode = ErrorCodes.OldPasswordInvalid) : base(message)
            => (ErrorCode) = (errorCode);
    }
}