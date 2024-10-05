using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

using UnityEngine;

namespace SpaceAce.Auxiliary
{
    public static class MyMath
    {
        #region random operations

        public static float RandomNormal => UnityEngine.Random.Range(-1f, 1f);
        public static float RandomUnit => UnityEngine.Random.Range(0f, 1f);
        public static bool RandomBool => RandomUnit > 0f;

        public static IEnumerable<int> GetValuesInRandomOrder(int minInclusive, int maxExclusive, IEnumerable<int> exclusions = null)
        {
            if (minInclusive >= maxExclusive)
                throw new ArgumentOutOfRangeException();

            int amount = exclusions is null ? maxExclusive - minInclusive
                                            : maxExclusive - minInclusive - exclusions.Count();

            return Enumerable.Range(minInclusive, maxExclusive - minInclusive)
                             .Except(exclusions)
                             .OrderBy(x => UnityEngine.Random.Range(0, amount));
        }

        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> collection)
        {
            if (collection is null)
                throw new ArgumentNullException();

            int amount = collection.Count();

            return collection.OrderBy(x => UnityEngine.Random.Range(0, amount));
        }

        #endregion

        #region range and interpolation operations

        public static bool ValueInRange(float value, float min, float max, float delta = 0f) =>
            value - delta >= min && value + delta <= max;

        public static bool ValueInRange(float value, UnityEngine.Vector2 range, float delta = 0f) =>
            value - delta >= range.x && value + delta <= range.y;

        public static bool ValueInRange(int value, int min, int max, int delta = 0) =>
            value - delta >= min && value + delta <= max;

        public static bool ValueInRange(int value, Vector2Int range, int delta = 0) =>
            value - delta >= range.x && value + delta <= range.y;

        public static Dictionary<UnityEngine.Vector2, T> InterpolateValuesByRange<T>(AnimationCurve interpolation, IEnumerable<T> values)
        {
            if (interpolation == null)
                throw new ArgumentNullException();

            if (values is null)
                throw new ArgumentNullException();

            int valuesAmount = values.Count();
            int counter = 0;

            Dictionary<UnityEngine.Vector2, T> result = new(valuesAmount);

            foreach (T value in values)
            {
                float rangeStart = interpolation.Evaluate((float)counter / valuesAmount);
                float rangeEnd = interpolation.Evaluate((float)(counter + 1) / valuesAmount);

                UnityEngine.Vector2 range = new(rangeStart, rangeEnd);
                result.Add(range, value);

                counter++;
            }

            return result;
        }

        public static Dictionary<UnityEngine.Vector2, T> InterpolateEnumByRange<T>(AnimationCurve interpolation, IEnumerable<T> customOrder = null) where T : Enum
        {
            T[] members;

            if (customOrder is null)
            {
                members = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            }
            else
            {
                members = customOrder.ToArray();
            }

            Dictionary<UnityEngine.Vector2, T> result = new(members.Length);

            for (int i = 0; i < members.Length; i++)
            {
                float rangeStart = interpolation.Evaluate((float)i / members.Length);
                float rangeEnd = interpolation.Evaluate((float)(i + 1) / members.Length);

                UnityEngine.Vector2 range = new(rangeStart, rangeEnd);
                T member = members[i];

                result.Add(range, member);
            }

            return result;
        }

        public static Dictionary<T, UnityEngine.Vector2> InterpolateRangeByEnum<T>(AnimationCurve interpolation, IEnumerable<T> customOrder = null) where T : Enum
        {
            if (interpolation == null)
                throw new ArgumentNullException();

            T[] members;

            if (customOrder is null)
            {
                members = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
            }
            else
            {
                members = customOrder.ToArray();
            }

            Dictionary<T, UnityEngine.Vector2> result = new(members.Length);

            for (int i = 0; i < members.Length; i++)
            {
                float rangeStart = interpolation.Evaluate((float)i / members.Length);
                float rangeEnd = interpolation.Evaluate((float)(i + 1) / members.Length);

                UnityEngine.Vector2 range = new(rangeStart, rangeEnd);
                T member = members[i];

                result.Add(member, range);
            }

            return result;
        }

        #endregion

        #region byte operations

        private const float AverageByteValue = 127.5f;

        public static void XORInternal(byte[] buffer, byte[] key)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                int value = buffer[i] ^ key[i % key.Length];
                buffer[i] = (byte)value;
            }
        }

        public static byte[] XOR(byte[] data, byte[] key)
        {
            byte[] result = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                int value = data[i] ^ key[i % key.Length];
                result[i] = (byte)value;
            }

            return result;
        }

        public static void ResetMany(params byte[][] buffers)
        {
            foreach (byte[] buffer in buffers)
                Reset(buffer);
        }

        public static void Reset(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = 0;
        }

        public static bool ContainsEqualValues(byte[] input, float fraction = 1f)
        {
            if (fraction <= 0f || fraction > 1f)
                throw new ArgumentOutOfRangeException();

            byte value = 0;
            float counter = 0f;

            do
            {
                foreach (byte b in input)
                    if (b == value)
                        counter++;

                if (counter / input.Length >= fraction)
                    return true;

                counter = 0f;
            }
            while (++value <= byte.MaxValue);

            return false;
        }

        public static float Randomness(byte[] input)
        {
            int sum = 0;

            foreach (byte b in input)
                sum += b;

            float averageSum = (float)sum / input.Length;
            float r = averageSum / AverageByteValue;

            return r > 1f ? 2f - r : r;
        }

        #endregion

        #region primes

        private static readonly HashSet<int> _primesUpTo100 = new()
        {
            2, 3, 5, 7,
            11, 13, 17, 19,
            23, 29,
            31, 37,
            41, 43, 47,
            53, 59,
            61, 67,
            71, 73, 79,
            83, 89,
            97
        };

        public static bool IsPrime(int number, float error = 1e-6f)
        {
            if (error <= 0f || error >= 1f)
                throw new ArgumentOutOfRangeException();

            if (number < 2)
                return false;

            if (_primesUpTo100.Contains(number) == true)
                return true;

            foreach (int p in _primesUpTo100)
                if (number % p == 0)
                    return false;

            (int power, int multiple) = DecomposeToPowerAndSmallestMultiple(number - 1, 2);
            int iterations = Mathf.CeilToInt(-1f * Mathf.Log(error, 4f));
            BigInteger x, y;

            for (int k = 0; k < iterations; k++)
            {
                int random = UnityEngine.Random.Range(2, number - 1);
                x = BigInteger.ModPow(random, multiple, number);

                for (int s = 0; s < power; s++)
                {
                    y = BigInteger.ModPow(x, 2, number);

                    if (y == 1 && x != 1 && x != number - 1)
                        return false;

                    x = y;
                }

                if (y != 1) return false;
            }

            return true;
        }

        public static int NearestPrimeBelow(int n)
        {
            if (n <= 2) throw new ArgumentOutOfRangeException();
            if (n == 3) return 2;

            int largestCandidate = n % 2 == 0 ? n - 1 : n - 2;

            for (int i = largestCandidate; i >= 3; i -= 2)
                if (IsPrime(i) == true)
                    return i;

            throw new ArithmeticException();
        }

        public static int NearestPrimeAbove(int n)
        {
            if (n < 2) return 2;

            int smallestCandidate = n % 2 == 0 ? n + 1 : n + 2;

            for (int i = smallestCandidate; ; i += 2)
                if (IsPrime(i) == true)
                    return i;
        }

        public static void PrimeTransform(byte[] buffer)
        {
            if (buffer.Length <= 2)
                throw new Exception("The buffer must be at least 3 bytes long!");

            int period = NearestPrimeBelow(buffer.Length);
            int delta = buffer.Length - period;

            for (int i = 0; i < buffer.Length; i++)
            {
                int t1 = (i + delta) % buffer.Length;
                int t1a = t1 == buffer.Length - 1 ? 0 : t1 + 1;
                int t1b = t1 == 0 ? buffer.Length - 1 : t1 - 1;

                int t2 = (i + delta + period - 1) % buffer.Length;
                int t2a = t2 == buffer.Length - 1 ? 0 : t1 + 1;
                int t2b = t2 == 0 ? buffer.Length - 1 : t2 - 1;

                int p = buffer[t1a] * buffer[t1b] * buffer[t2a] * buffer[t2b] + 1;

                buffer[i] = IsPrime(p) == true ? (byte)(buffer[t1] - buffer[t2])
                                               : (byte)(buffer[t1] + buffer[t2]);
            }
        }

        #endregion

        #region arithmetic functions

        public static (int power, int mulpiple) DecomposeToPowerAndSmallestMultiple(int number, int basis)
        {
            if (number <= 0 || basis <= 1)
                throw new ArgumentOutOfRangeException();

            if (number % basis != 0 || basis > number)
                return (0, number);

            int residue = number;
            int power = 0;

            while (residue % basis == 0)
            {
                residue /= basis;
                power++;
            }

            return (power, residue);
        }

        #endregion
    }
}