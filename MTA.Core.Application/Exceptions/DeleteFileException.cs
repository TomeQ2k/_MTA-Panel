using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class DeleteFileException : ApplicationException
    {
        public string ErrorCode { get; }

        public DeleteFileException(string message, string errorCode = ErrorCodes.DeleteFileFailed) : base(message)
            => (ErrorCode) = (errorCode);
    }
}