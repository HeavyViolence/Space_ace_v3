using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SpaceAce.Auxiliary
{
    public static class MyMath
    {
        #region random operations

        public static float RandomUnit => UnityEngine.Random.Range(0f, 1f);
        public static Vector2 RandomUnit2D => new(RandomUnit, RandomUnit);
        public static Vector3 RandomUnit3D => new(RandomUnit, RandomUnit, RandomUnit);

        public static float RandomNormal => UnityEngine.Random.Range(-1f, 1f);
        public static Vector2 RandomNormal2D => new(RandomNormal, RandomNormal);
        public static Vector3 RandomNormal3D => new(RandomNormal, RandomNormal, RandomNormal);

        public static float RandomSign => RandomNormal > 0f ? 1f : -1f;
        public static bool RandomBool => RandomNormal > 0f;

        public static IEnumerable<int> GetValuesInRandomOrder(int minInclusive, int maxExclusive, IEnumerable<int> exclusions = null)
        {
            if (minInclusive >= maxExclusive)
            {
                throw new ArgumentOutOfRangeException();
            }

            int count = exclusions is null ? maxExclusive - minInclusive
                                           : maxExclusive - minInclusive - exclusions.Count();

            return Enumerable.Range(minInclusive, maxExclusive - minInclusive)
                             .Except(exclusions ?? Enumerable.Empty<int>())
                             .OrderBy(x => UnityEngine.Random.Range(0, count));
        }

        public static IEnumerable<T> Shuffle<T>(IEnumerable<T> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException();
            }

            int count = collection.Count();

            if (count == 1)
            {
                throw new Exception("Collection to shuffle must contain more than 1 element!");
            }

            return collection.OrderBy(x => UnityEngine.Random.Range(0, count));
        }

        public static T GetRandom<T>(IEnumerable<T> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException();
            }

            int count = collection.Count();

            if (count == 1)
            {
                return collection.FirstOrDefault();
            }

            int randomIndex = UnityEngine.Random.Range(0, count);
            return collection.ElementAtOrDefault(randomIndex);
        }

        public static KeyValuePair<K, V> GetRandom<K, V>(IEnumerable<KeyValuePair<K, V>> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException();
            }

            int count = collection.Count();

            if (count == 1)
            {
                return collection.FirstOrDefault();
            }

            int randomIndex = UnityEngine.Random.Range(0, count);
            return collection.ElementAtOrDefault(randomIndex);
        }

        #endregion

        #region range and interpolation operations

        public static bool ValueInRange(float value, float min, float max, float delta = 0f) =>
            value >= min - delta && value <= max + delta;

        public static bool ValueInRange(float value, Vector2 range, float delta = 0f) =>
            value >= range.x - delta && value <= range.y + delta;

        public static bool ValueInRange(int value, int min, int max, int delta = 0) =>
            value >= min - delta && value <= max + delta;

        public static bool ValueInRange(int value, Vector2Int range, int delta = 0) =>
            value >= range.x - delta && value <= range.y + delta;

        public static Dictionary<Vector2, T> InterpolateValuesByRange<T>(AnimationCurve interpolation, IEnumerable<T> values)
        {
            if (interpolation == null || values is null)
            {
                throw new ArgumentNullException();
            }

            int count = values.Count();
            int counter = 0;

            Dictionary<Vector2, T> result = new(count);

            foreach (T value in values)
            {
                float rangeStart = interpolation.Evaluate((float)counter / count);
                float rangeEnd = interpolation.Evaluate((float)(counter + 1) / count);

                Vector2 range = new(rangeStart, rangeEnd);
                result.Add(range, value);

                counter++;
            }

            return result;
        }

        public static Dictionary<Vector2, T> InterpolateEnumByRange<T>(AnimationCurve interpolation, IEnumerable<T> customOrder = null) where T : Enum
        {
            T[] members = customOrder is null ? Enum.GetValues(typeof(T)).Cast<T>().ToArray()
                                              : customOrder.ToArray();

            Dictionary<Vector2, T> result = new(members.Length);

            for (int i = 0; i < members.Length; i++)
            {
                float rangeStart = interpolation.Evaluate((float)i / members.Length);
                float rangeEnd = interpolation.Evaluate((float)(i + 1) / members.Length);

                Vector2 range = new(rangeStart, rangeEnd);
                T member = members[i];

                result.Add(range, member);
            }

            return result;
        }

        public static Dictionary<T, Vector2> InterpolateRangeByEnum<T>(AnimationCurve interpolation, IEnumerable<T> customOrder = null) where T : Enum
        {
            if (interpolation == null)
            {
                throw new ArgumentNullException();
            }

            T[] members = customOrder is null ? Enum.GetValues(typeof(T)).Cast<T>().ToArray()
                                              : customOrder.ToArray();

            Dictionary<T, Vector2> result = new(members.Length);

            for (int i = 0; i < members.Length; i++)
            {
                float rangeStart = interpolation.Evaluate((float)i / members.Length);
                float rangeEnd = interpolation.Evaluate((float)(i + 1) / members.Length);

                Vector2 range = new(rangeStart, rangeEnd);
                T member = members[i];

                result.Add(member, range);
            }

            return result;
        }

        public static float Remap(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            float t = Mathf.InverseLerp(oldMin, oldMax, value);
            float result = Mathf.Lerp(newMin, newMax, t);

            return result;
        }

        public static int Remap(int value, int oldMin, int oldMax, int newMin, int newMax)
        {
            float t = (float)(value - oldMin) / (oldMax - oldMin);
            int result = (int)(newMin + (newMax - newMin) * t);

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
            {
                Reset(buffer);
            }
        }

        public static void Reset(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0;
            }
        }

        public static bool ContainsEqualValues(byte[] input, float fraction = 1f)
        {
            if (fraction <= 0f || fraction > 1f)
            {
                throw new ArgumentOutOfRangeException();
            }

            byte value = 0;
            float counter = 0f;

            do
            {
                foreach (byte b in input)
                {
                    if (b == value)
                    {
                        counter++;
                    }
                }

                if (counter / input.Length >= fraction)
                {
                    return true;
                }

                counter = 0f;
            }
            while (++value <= byte.MaxValue);

            return false;
        }

        public static float CalculateRandomness(byte[] input)
        {
            int sum = 0;

            foreach (byte b in input)
            {
                sum += b;
            }

            float averageSum = (float)sum / input.Length;
            float r = averageSum / AverageByteValue;

            return r > 1f ? 2f - r : r;
        }

        public static void ArithmeticTransform(byte[] input)
        {
            int length = input.Length;

            if (length < 3)
            {
                throw new Exception("Input must be at least 3 bytes long!");
            }

            for (int i = 0; i < length; i++)
            {
                int index1 = i;
                int index2 = (i + length - 1) % length;
                int index3 = (i + 1) % length;

                byte value1 = input[index1];
                byte value2 = input[index2];
                bool t = input[index3] % 3 == 0;

                input[i] = t == true ? (byte)(value1 + value2) : (byte)(value1 - value2);
            }
        }

        #endregion
    }
}