using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class ResetPasswordException : ApplicationException
    {
        public string ErrorCode { get; }

        public ResetPasswordException(string message, string errorCode = ErrorCodes.ResetPasswordFailed) : base(message)
            => (ErrorCode) = (errorCode);
    }
}