using SpaceAce.Auxiliary;
using SpaceAce.Auxiliary.Exceptions;

using System;

namespace SpaceAce.Main.Saving
{
    public sealed class XOREncryptor : Encryptor
    {
        public XOREncryptor(IKeyValidator validator) : base(validator) { }

        public override byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            if (data is null)
            {
                throw new ArgumentNullException();
            }

            if (KeyValidator.IsValidKey(key) == false)
            {
                throw new InvalidKeyException();
            }

            if (KeyValidator.IsValidIV(iv) == false)
            {
                throw new InvalidIVException();
            }

            byte[] initializedKey = MyMath.XOR(key, iv);

            try
            {
                return MyMath.XOR(data, initializedKey);
            }
            finally
            {
                MyMath.Reset(initializedKey);
            }
        }

        public override byte[] Decrypt(byte[] data, byte[] key, byte[] iv) =>
            Encrypt(data, key, iv);
    }
}