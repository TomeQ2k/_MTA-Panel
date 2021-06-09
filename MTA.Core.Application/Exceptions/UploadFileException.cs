using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class UploadFileException : ApplicationException
    {
        public string ErrorCode { get; }

        public UploadFileException(string message = ErrorMessages.UploadingFilesErrorMessage,
            string errorCode = ErrorCodes.UploadFileFailed) : base(message)
            => (ErrorCode) = (errorCode);
    }
}