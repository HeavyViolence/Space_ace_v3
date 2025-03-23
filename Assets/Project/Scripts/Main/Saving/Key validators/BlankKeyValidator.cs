namespace SpaceAce.Main.Saving
{
    public sealed class BlankKeyValidator : KeyValidator
    {
        public override bool IsValidKey(byte[] key) => true;
        public override bool IsValidIV(byte[] iv) => true;
    }
}