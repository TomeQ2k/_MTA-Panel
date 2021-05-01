using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Services;

namespace MTA.Infrastructure.Shared.Services
{
    public class HashGenerator : IHashGenerator
    {
        public string GenerateHash(string password, string passwordSalt)
        {
            var passwordBinary = Encoding.UTF8.GetBytes(password);
            var passwordSaltBinary = Encoding.UTF8.GetBytes(passwordSalt);

            using (MD5 md5 = MD5.Create())
            {
                var passwordHashBinary =
                    md5.ComputeHash(CombineByteArrays(passwordBinary, passwordSaltBinary));
                return passwordHashBinary.ConvertHashToString();
            }
        }

        public string CreateSalt(int saltSize = 128)
        {
            var rngCryptoService = new RNGCryptoServiceProvider();

            byte[] saltBinary = new byte[saltSize];
            rngCryptoService.GetBytes(saltBinary);

            return BitConverter.ToString(saltBinary).Replace("-", "");
        }

        public byte[] CombineByteArrays(params byte[][] arrays)
        {
            byte[] resultArray = new byte[arrays.Sum(x => x.Length)];
            int offset = 0;

            foreach (byte[] data in arrays)
            {
                Buffer.BlockCopy(data, 0, resultArray, offset, data.Length);
                offset += data.Length;
            }

            return resultArray;
        }

        public bool VerifyHash(string text, string textSalt, string textHash)
        {
            var hashToCheck = GenerateHash(text, textSalt);

            for (int i = 0; i < hashToCheck.Length; i++)
                if (hashToCheck[i] != textHash[i])
                    return false;

            return true;
        }
    }
}