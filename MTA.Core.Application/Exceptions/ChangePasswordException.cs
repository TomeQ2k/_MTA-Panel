using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class ChangePasswordException : Exception
    {
        public string ErrorCode { get; }

        public ChangePasswordException(string message, string errorCode = ErrorCodes.ChangePasswordFailed) : base(message)
            => (ErrorCode) = (errorCode);
    }
}