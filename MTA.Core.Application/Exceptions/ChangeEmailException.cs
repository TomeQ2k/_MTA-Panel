﻿using System;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Exceptions
{
    public class ChangeEmailException : ApplicationException
    {
        public string ErrorCode { get; }

        public ChangeEmailException(string message, string errorCode = ErrorCodes.ChangePasswordFailed) :
            base(message)
            => (ErrorCode) = (errorCode);
    }
}