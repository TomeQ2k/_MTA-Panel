using System.Text.RegularExpressions;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Extensions
{
    public static class StringExtensions
    {
        public static bool HasWhitespaces(this string value)
            => string.IsNullOrWhiteSpace(value) || value.Contains(" ");

        public static bool IsEmailAddress(this string value)
            => !string.IsNullOrEmpty(value) && Regex.Match(value, Constants.IsEmailAddressRegex).Success;

        public static string RemoveLastCharacter(this string str) => str.Remove(str.Length - 1);
    }
}
