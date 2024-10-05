namespace SpaceAce.Main.Saving
{
    public sealed class BlankKeyGenerator : IKeyGenerator
    {
        private static readonly byte[] _dummy = new byte[0];

        public int KeySize => 0;
        public int IVSize => 0;

        public byte[] GenerateKey(string savedDataName) => _dummy;
        public byte[] GenerateIV() => _dummy;
    }
}