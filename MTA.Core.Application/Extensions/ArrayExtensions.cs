using System.Text;

namespace MTA.Core.Application.Extensions
{
    public static class ArrayExtensions
    {
        public static string ConvertHashToString(this byte[] hashBytes, int size = 128)
        {
            var stringBuilder = new StringBuilder(size);

            foreach (var hashByte in hashBytes)
                stringBuilder.Append(hashByte.ToString("X2"));

            return stringBuilder.ToString();
        }
    }
}