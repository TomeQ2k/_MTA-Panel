namespace MTA.Core.Common.Helpers
{
    public static class ErrorMessages
    {
        public const string DatabaseErrorMessage = "Database operation has failed";
        public const string DatabaseTransactionErrorMessage = "Database transaction operation has failed";
        public const string NotAllowedMessage = "You are not allowed to perform this action";
        public const string TokenExpiredErrorMessage = "Token has expired";
        public const string CaptchaInvalidErrorMessage = "Captcha processing error";
        public const string UploadingFilesErrorMessage = "Error occurred during uploading files on server";
        public const string PaypalErrorMessage = "Error occurred during paypal operation";
    }
}