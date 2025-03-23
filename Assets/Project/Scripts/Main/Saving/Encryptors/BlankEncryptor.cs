namespace SpaceAce.Main.Saving
{
    public sealed class BlankEncryptor : Encryptor
    {
        public BlankEncryptor(KeyValidator validator) : base(validator) { }

        public override byte[] Encrypt(byte[] data, byte[] key, byte[] iv) => data;
        public override byte[] Decrypt(byte[] data, byte[] key, byte[] iv) => data;
    }
}