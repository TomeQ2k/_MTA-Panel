using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class UploadFileException : Exception
    {
        public string ErrorCode { get; }

        public UploadFileException(string message = ErrorMessages.UploadingFilesErrorMessage,
            string errorCode = ErrorCodes.UploadFileFailed) : base(message)
            => (ErrorCode) = (errorCode);
    }
}