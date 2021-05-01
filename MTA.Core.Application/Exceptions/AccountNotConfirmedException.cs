using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class AccountNotConfirmedException : Exception
    {
        public string ErrorCode { get; }

        public AccountNotConfirmedException(string message, string errorCode = ErrorCodes.AccountNotConfirmed) : base(message)
            => (ErrorCode) = (errorCode);
    }
}