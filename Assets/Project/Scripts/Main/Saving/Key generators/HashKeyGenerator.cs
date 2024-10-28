using System;
using System.Security.Cryptography;
using System.Text;

namespace SpaceAce.Main.Saving
{
    public sealed class HashKeyGenerator : IKeyGenerator
    {
        private static readonly UTF8Encoding _utf8 = new(true, true);
        private static readonly SHA256 _sha256 = SHA256.Create();
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        public int KeySize => 32;
        public int IVSize => 32;

        public byte[] GenerateKey(string seed)
        {
            if (string.IsNullOrEmpty(seed) == true ||
                string.IsNullOrWhiteSpace(seed) == true)
            {
                throw new ArgumentNullException();
            }

            byte[] data = _utf8.GetBytes(seed);
            byte[] hash = _sha256.ComputeHash(data);

            return hash;
        }

        public byte[] GenerateIV()
        {
            byte[] iv = new byte[IVSize];
            _rng.GetBytes(iv);

            return iv;
        }
    }
}