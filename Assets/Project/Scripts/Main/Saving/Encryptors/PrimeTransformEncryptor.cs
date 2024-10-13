using SpaceAce.Auxiliary;
using SpaceAce.Auxiliary.Exceptions;

using System;

namespace SpaceAce.Main.Saving
{
    public sealed class PrimeTransformEncryptor : Encryptor
    {
        public PrimeTransformEncryptor(IKeyValidator validator) : base(validator) { }

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
                byte[] result = new byte[data.Length];

                for (int i = 0; i < data.Length; i++)
                {
                    int keyIndex = i % initializedKey.Length;

                    if (keyIndex == 0 && i > 0)
                    {
                        MyMath.PrimeTransform(initializedKey);
                    }

                    int b = data[i] ^ initializedKey[keyIndex];
                    result[i] = (byte)b;
                }

                return result;
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