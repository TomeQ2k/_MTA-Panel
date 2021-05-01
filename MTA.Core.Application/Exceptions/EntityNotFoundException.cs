using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string ErrorCode { get; }

        public EntityNotFoundException(string message, string errorCode = ErrorCodes.EntityNotFound) : base(message)
            => (ErrorCode) = (errorCode);
    }
}