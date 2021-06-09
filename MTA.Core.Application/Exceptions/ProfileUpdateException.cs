using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class ProfileUpdateException : ApplicationException
    {
        public string ErrorCode { get; }

        public ProfileUpdateException(string message, string errorCode = ErrorCodes.ProfileUpdateError) : base(message)
            => (ErrorCode) = (errorCode);
    }
}