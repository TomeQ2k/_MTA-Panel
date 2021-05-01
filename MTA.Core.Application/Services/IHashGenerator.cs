namespace MTA.Core.Application.Services
{
    public interface IHashGenerator
    {
        public string GenerateHash(string password, string passwordSalt);
        public string CreateSalt(int saltSize = 128);
        public byte[] CombineByteArrays(params byte[][] arrays);
        public bool VerifyHash(string text, string textSalt, string textHash);
    }
}