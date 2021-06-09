using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class CrudException : ApplicationException
    {
        public string ErrorCode { get; }

        public CrudException(string message, string errorCode = ErrorCodes.CrudActionFailed) : base(message)
            => (ErrorCode) = (errorCode);
    }
}