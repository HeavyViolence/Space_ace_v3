using System;

namespace SpaceAce.Main.Saving
{
    public abstract class Encryptor
    {
        protected readonly IKeyValidator KeyValidator;

        public Encryptor(IKeyValidator validator)
        {
            KeyValidator = validator ?? throw new ArgumentNullException();
        }

        public abstract byte[] Encrypt(byte[] data, byte[] key, byte[] iv);
        public abstract byte[] Decrypt(byte[] data, byte[] key, byte[] iv);
    }
}