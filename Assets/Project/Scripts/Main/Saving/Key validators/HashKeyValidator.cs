using SpaceAce.Auxiliary;

using System;

namespace SpaceAce.Main.Saving
{
    public sealed class HashKeyValidator : KeyValidator
    {
        private const int ValidKeySize = 32;
        private const float MinKeyRandomness = 0.5f;

        private const int ValidIVSize = 32;
        private const float MinIVRandomness = 0.5f;

        public override bool IsValidKey(byte[] key)
        {
            if (key is null)
            {
                throw new ArgumentNullException();
            }

            if (key.Length != ValidKeySize)
            {
                return false;
            }

            if (MyMath.CalculateRandomness(key) < MinKeyRandomness)
            {
                return false;
            }

            return true;
        }

        public override bool IsValidIV(byte[] iv)
        {
            if (iv is null)
            {
                throw new ArgumentNullException();
            }

            if (iv.Length != ValidIVSize)
            {
                return false;
            }

            if (MyMath.CalculateRandomness(iv) < MinIVRandomness)
            {
                return false;
            }

            return true;
        }
    }
}