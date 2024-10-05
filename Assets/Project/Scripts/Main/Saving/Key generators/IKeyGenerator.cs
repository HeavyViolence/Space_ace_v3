namespace SpaceAce.Main.Saving
{
    public interface IKeyGenerator
    {
        int KeySize { get; }
        int IVSize { get; }

        byte[] GenerateKey(string savedDataName);
        byte[] GenerateIV();
    }
}