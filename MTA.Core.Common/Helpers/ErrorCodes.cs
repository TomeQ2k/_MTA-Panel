namespace MTA.Core.Common.Helpers
{
    public static class ErrorCodes
    {
        public const string ServerError = "SERVER_ERROR";
        public const string CrudActionFailed = "CRUD_ACTION_FAILED";
        public const string ValidationError = "VALIDATION_ERROR";
        public const string DatabaseError = "DATABASE_ERROR";
        public const string ServiceError = "SERVICE_UNAVAILABLE";
        public const string EntityNotFound = "ENTITY_NOT_FOUND";
        public const string AccessDenied = "ACCESS_DENIED";
        public const string DuplicateExists = "DUPLICATE_EXISTS";
        public const string AccountBlocked = "ACCOUNT_BLOCKED";
        public const string PaymentError = "PAYMENT_ERROR";
        public const string PremiumError = "PREMIUM_OPERATION_ERROR";
        public const string CaptchaError = "CAPTCHA_ERROR";

        public const string AuthError = "AUTHENTICATION_ERROR";
        public const string InvalidCredentials = "INVALID_CREDENTIALS";
        public const string AccountNotConfirmed = "ACCOUNT_NOT_CONFIRMED";
        public const string EmailExists = "EMAIL_EXISTS";
        public const string UsernameExists = "USERNAME_EXISTS";
        public const string ResetPasswordFailed = "RESET_PASSWORD_FAILED";

        public const string CannotGenerateToken = "GENERATE_TOKEN_FAILED";
        public const string TokenExpired = "TOKEN_EXPIRED";

        public const string ProfileUpdateError = "PROFILE_UPDATE_ERROR";
        public const string ChangePasswordFailed = "CHANGE_PASSWORD_FAILED";
        public const string ChangeEmailFailed = "CHANGE_EMAIL_FAILED";
        public const string OldPasswordInvalid = "OLD_PASSWORD_INVALID";

        public const string UploadFileFailed = "UPLOAD_FILE_FAILED";
        public const string DeleteFileFailed = "DELETE_FILE_FAILED";
    }
}