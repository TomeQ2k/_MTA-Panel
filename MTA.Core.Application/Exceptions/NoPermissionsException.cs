using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class NoPermissionsException : Exception
    {
        public string ErrorCode { get; }

        public NoPermissionsException(string message, string errorCode = ErrorCodes.AccessDenied) : base(message)
            => (ErrorCode) = (errorCode);
    }
}