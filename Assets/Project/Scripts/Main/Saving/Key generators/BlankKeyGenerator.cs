namespace SpaceAce.Main.Saving
{
    public sealed class BlankKeyGenerator : KeyGenerator
    {
        private static readonly byte[] s_dummy = new byte[0];

        public override int KeySize => 0;
        public override int IVSize => 0;

        public override byte[] GenerateKey(string seed) => s_dummy;
        public override byte[] GenerateIV() => s_dummy;
    }
}