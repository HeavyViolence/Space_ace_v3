using SpaceAce.Auxiliary.Exceptions;

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SpaceAce.Auxiliary.SafeValues
{
    public sealed class SafeFloat : IDisposable,
                                    IEquatable<SafeFloat>,
                                    IComparable<SafeFloat>,
                                    IComparer<SafeFloat>
    {
        private const int BytesPerFloat = 4;

        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        private byte[] _value = new byte[BytesPerFloat];
        private readonly byte[] _iv = new byte[BytesPerFloat];
        private bool _disposed = false;

        public SafeFloat(float value = 0f)
        {
            _value = BitConverter.GetBytes(value);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);
        }

        public float GetValue()
        {
            if (_disposed == true)
                throw new DisposedException();

            try
            {
                MyMath.XORInternal(_value, _iv);
                return BitConverter.ToSingle(_value, 0);
            }
            finally
            {
                _rng.GetBytes(_iv);
                MyMath.XORInternal(_value, _iv);
            }
        }

        public void SetValue(float value)
        {
            if (_disposed == true)
                throw new DisposedException();

            _value = BitConverter.GetBytes(value);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);
        }

        public void Add(float value)
        {
            if (_disposed == true)
                throw new DisposedException();

            MyMath.XORInternal(_value, _iv);

            float oldValue = BitConverter.ToSingle(_value);
            float newValue = oldValue + value;

            SetValue(newValue);
        }

        public void Subtract(float value)
        {
            if (_disposed == true)
                throw new DisposedException();

            MyMath.XORInternal(_value, _iv);

            float oldValue = BitConverter.ToSingle(_value);
            float newValue = oldValue - value;

            SetValue(newValue);
        }

        public void Multiply(float value)
        {
            if (_disposed == true)
                throw new DisposedException();

            MyMath.XORInternal(_value, _iv);

            float oldValue = BitConverter.ToSingle(_value);
            float newValue = oldValue * value;

            SetValue(newValue);
        }

        public void Divide(float value)
        {
            if (_disposed == true)
                throw new DisposedException();

            if (value == 0f)
                throw new ArgumentOutOfRangeException();

            MyMath.XORInternal(_value, _iv);

            float oldValue = BitConverter.ToSingle(_value);
            float newValue = oldValue / value;

            SetValue(newValue);
        }

        #region interfaces

        public void Dispose()
        {
            if (_disposed == true)
                throw new DisposedException();

            MyMath.Reset(_value);
            MyMath.Reset(_iv);

            _disposed = true;
        }

        public override bool Equals(object obj) =>
            obj is not null && Equals(obj as SafeFloat) == true;

        public bool Equals(SafeFloat other) =>
            GetValue() == other.GetValue();

        public override int GetHashCode() =>
            _value.GetHashCode() ^ _iv.GetHashCode();

        public int CompareTo(SafeFloat other)
        {
            if (other is null)
                throw new ArgumentNullException();

            if (GetValue() < other.GetValue()) return -1;
            if (GetValue() > other.GetValue()) return 1;

            return 0;
        }

        public int Compare(SafeFloat x, SafeFloat y)
        {
            if (x is null || y is null)
                throw new ArgumentNullException();

            if (x.GetValue() < y.GetValue()) return -1;
            if (x.GetValue() > y.GetValue()) return 1;

            return 0;
        }

        #endregion
    }
}