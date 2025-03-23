namespace SpaceAce.Main.Saving
{
    public abstract class KeyValidator
    {
        public abstract bool IsValidKey(byte[] key);
        public abstract bool IsValidIV(byte[] iv);
    }
}