using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class DatabaseException : ApplicationException
    {
        public string ErrorCode { get; }

        public DatabaseException(string message = ErrorMessages.DatabaseErrorMessage,
            string errorCode = ErrorCodes.DatabaseError) : base(message)
            => (ErrorCode) = (errorCode);
    }
}