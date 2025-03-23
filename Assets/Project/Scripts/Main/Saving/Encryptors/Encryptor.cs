using System;

namespace SpaceAce.Main.Saving
{
    public abstract class Encryptor
    {
        protected readonly KeyValidator KeyValidator;

        public Encryptor(KeyValidator validator)
        {
            KeyValidator = validator ?? throw new ArgumentNullException();
        }

        public abstract byte[] Encrypt(byte[] data, byte[] key, byte[] iv);
        public abstract byte[] Decrypt(byte[] data, byte[] key, byte[] iv);
    }
}