namespace SpaceAce.Main.Saving
{
    public abstract class KeyGenerator
    {
        public abstract int KeySize { get; }
        public abstract int IVSize { get; }

        public abstract byte[] GenerateKey(string seed);
        public abstract byte[] GenerateIV();
    }
}