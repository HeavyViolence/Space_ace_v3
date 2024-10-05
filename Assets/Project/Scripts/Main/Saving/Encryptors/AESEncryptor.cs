using SpaceAce.Auxiliary.Exceptions;

using System;
using System.Security.Cryptography;

namespace SpaceAce.Main.Saving
{
    public sealed class AESEncryptor : Encryptor
    {
        public AESEncryptor(IKeyValidator validator) : base(validator) { }

        public override byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            if (data is null)
                throw new ArgumentNullException();

            if (KeyValidator.IsValidKey(key) == false)
                throw new InvalidKeyException();

            if (KeyValidator.IsValidIV(iv) == false)
                throw new InvalidIVException();

            using Aes algorithm = Aes.Create();
            using ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv);

            try
            {
                byte[] result = encryptor.TransformFinalBlock(data, 0, data.Length);
                return result;
            }
            finally
            {
                algorithm.Clear();
            }
        }

        public override byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            if (data is null)
                throw new ArgumentNullException();

            if (KeyValidator.IsValidKey(key) == false)
                throw new InvalidKeyException();

            if (KeyValidator.IsValidIV(iv) == false)
                throw new InvalidIVException();

            using Aes algorithm = Aes.Create();
            using ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv);

            try
            {
                byte[] result = decryptor.TransformFinalBlock(data, 0, data.Length);
                return result;
            }
            finally
            {
                algorithm.Clear();
            }
        }
    }
}