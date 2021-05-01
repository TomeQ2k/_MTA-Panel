using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class ServiceException : Exception
    {
        public string ErrorCode { get; }

        public ServiceException(string message, string errorCode = ErrorCodes.ServiceError) : base(message)
            => (ErrorCode) = (errorCode);
    }
}