namespace SpaceAce.Main.Saving
{
    public sealed class BlankKeyValidator : IKeyValidator
    {
        public bool IsValidKey(byte[] key) => true;
        public bool IsValidIV(byte[] iv) => true;
    }
}