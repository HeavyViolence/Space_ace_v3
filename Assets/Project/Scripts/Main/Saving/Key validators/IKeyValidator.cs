namespace SpaceAce.Main.Saving
{
    public interface IKeyValidator
    {
        bool IsValidKey(byte[] key);
        bool IsValidIV(byte[] iv);
    }
}