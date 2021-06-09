using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class ServerException : ApplicationException
    {
        public string ErrorCode { get; }

        public ServerException(string message, string errorCode = ErrorCodes.ServerError) : base(message)
            => (ErrorCode) = (errorCode);
    }
}