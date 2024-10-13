using SpaceAce.Auxiliary;

using System;

namespace SpaceAce.Main.Saving
{
    public sealed class AESKeyValidator : IKeyValidator
    {
        private const int ValidKeySize = 32;
        private const float MinKeyRandomness = 0.5f;

        private const int ValidIVSize = 16;
        private const float MinIVRandomness = 0.3f;

        public bool IsValidKey(byte[] key)
        {
            if (key is null)
            {
                throw new ArgumentNullException();
            }

            if (key.Length != ValidKeySize)
            {
                return false;
            }

            if (MyMath.Randomness(key) < MinKeyRandomness)
            {
                return false;
            }

            return true;
        }

        public bool IsValidIV(byte[] iv)
        {
            if (iv is null)
            {
                throw new ArgumentNullException();
            }

            if (iv.Length != ValidIVSize)
            {
                return false;
            }

            if (MyMath.Randomness(iv) < MinIVRandomness)
            {
                return false;
            }

            return true;
        }
    }
}