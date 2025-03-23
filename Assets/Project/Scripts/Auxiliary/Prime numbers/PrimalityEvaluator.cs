using System;
using System.Collections.Generic;

using VContainer;

namespace SpaceAce.Auxiliary.PrimeNumbers
{
    public sealed class PrimalityEvaluator
    {
        public const int MinIterations = 3;
        public const int MaxIterations = 30;

        private static readonly Random _random = new();

        private static readonly HashSet<int> s_primesUpTo100 = new()
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

        public readonly int Iterations;
        public readonly float Error;

        [Inject]
        public PrimalityEvaluator(int iterations)
        {
            Iterations = MyMath.ValueInRange(iterations, MinIterations, MaxIterations)
                ? iterations
                : throw new ArgumentOutOfRangeException(
                    $"Iterations must be in the following range: [{MinIterations}; {MaxIterations}]!");

            Error = MathF.Pow(4f, -1f * Iterations);
        }

        public bool IsPrime(int n)
        {
            if (n < 2)
            {
                return false;
            }

            if (s_primesUpTo100.Contains(n) == true)
            {
                return true;
            }

            foreach (int p in s_primesUpTo100)
            {
                if (n % p == 0)
                {
                    return false;
                }
            }

            int d = n - 1;
            int s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s++;
            }

            for (int i = 0; i < Iterations; i++)
            {
                int a = _random.Next(2, n - 2);
                int x = ModularExponent(a, d, n);

                if (x == 1 || x == n - 1)
                {
                    continue;
                }

                for (int j = 0; j < s - 1; j++)
                {
                    x = ModularExponent(x, 2, n);

                    if (x == 1)
                    {
                        return false;
                    }

                    if (x == n - 1)
                    {
                        break;
                    }
                }

                if (x != n - 1)
                {
                    return false;
                }
            }

            return true;
        }

        public int NearestPrimeBelow(int n)
        {
            if (n <= 2)
            {
                throw new ArithmeticException();
            }

            if (n == 3)
            {
                return 2;
            }

            int largestCandidate = n % 2 == 0 ? n - 1 : n - 2;

            for (int i = largestCandidate; i >= 3; i -= 2)
            {
                if (IsPrime(i) == true)
                {
                    return i;
                }
            }

            throw new ArithmeticException();
        }

        public int NearestPrimeAbove(int n)
        {
            if (n < 2)
            {
                return 2;
            }

            int smallestCandidate = n % 2 == 0 ? n + 1 : n + 2;

            for (int i = smallestCandidate; i <= int.MaxValue; i += 2)
            {
                if (IsPrime(i) == true)
                {
                    return i;
                }
            }

            throw new ArithmeticException();
        }

        public int RandomPrime()
        {
            int randomNumber = _random.Next(3, int.MaxValue);
            return NearestPrimeBelow(randomNumber);
        }

        private int ModularExponent(int basis, int power, int modulo)
        {
            int result = 1;
            basis %= modulo;

            while (power > 0)
            {
                if (power % 2 == 1)
                {
                    result = (result * basis) % modulo;
                }

                basis = (basis * basis) % modulo;
                power /= 2;
            }

            return result;
        }
    }
}