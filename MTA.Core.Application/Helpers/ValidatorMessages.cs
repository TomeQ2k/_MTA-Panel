namespace MTA.Core.Application.Helpers
{
    public static class ValidatorMessages
    {
        public const string MainValidatorMessage = "Invalid input data";

        public const string WhitespacesNotAllowedValidatorMessage = "Whitespaces are not allowed";
        public const string EmailAddressValidatorMessage = "Invalid email address";

        public static string MaxFileSizeValidatorMessage(int maxFileSize)
            => $"Maximum file size is: {maxFileSize} Bytes";

        public static string MaxFilesCountValidatorMessage(int maxFilesCount)
            => $"Maximum files count is: {maxFilesCount}";

        public static string FileExtensionsValidatorMessage(string[] extensions)
        {
            var errorMessage = $"Allowed file extensions are: ";

            for (int i = 0; i < extensions.Length; i++)
                errorMessage += i != extensions.Length - 1 ? $"{extensions[i]}, " : extensions[i];

            return errorMessage;
        }
    }
}