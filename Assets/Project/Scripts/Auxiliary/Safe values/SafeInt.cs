using SpaceAce.Auxiliary.Exceptions;
using SpaceAce.Auxiliary.Observables;

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SpaceAce.Auxiliary.SafeValues
{
    public sealed class SafeInt : IDisposable,
                                  IEquatable<SafeInt>,
                                  IComparable<SafeInt>,
                                  IComparer<SafeInt>,
                                  IObservable<int>
    {
        private const int BytesPerInt = 4;

        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        private byte[] _value;
        private readonly byte[] _iv = new byte[BytesPerInt];
        private bool _disposed = false;

        private readonly ValueTracker<int> _valueTracker = new();

        public SafeInt(int value = 0)
        {
            _value = BitConverter.GetBytes(value);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);
        }

        public int GetValue()
        {
            if (_disposed == true)
                throw new DisposedException();

            try
            {
                MyMath.XORInternal(_value, _iv);
                return BitConverter.ToInt32(_value, 0);
            }
            finally
            {
                _rng.GetBytes(_iv);
                MyMath.XORInternal(_value, _iv);
            }
        }

        public void SetValue(int value)
        {
            if (_disposed == true)
                throw new DisposedException();

            _value = BitConverter.GetBytes(value);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);

            _valueTracker.Track(value);
        }

        public void Add(int value)
        {
            if (_disposed == true)
                throw new DisposedException();

            MyMath.XORInternal(_value, _iv);

            int oldValue = BitConverter.ToInt32(_value);
            int newValue = oldValue + value;

            SetValue(newValue);
        }

        public void Subtract(int value)
        {
            if (_disposed == true)
                throw new DisposedException();

            MyMath.XORInternal(_value, _iv);

            int oldValue = BitConverter.ToInt32(_value);
            int newValue = oldValue - value;

            SetValue(newValue);
        }

        public void Multiply(int value)
        {
            if (_disposed == true)
                throw new DisposedException();

            MyMath.XORInternal(_value, _iv);

            int oldValue = BitConverter.ToInt32(_value);
            int newValue = oldValue * value;

            SetValue(newValue);
        }

        public void Divide(int value)
        {
            if (_disposed == true)
                throw new DisposedException();

            if (value == 0)
                throw new ArgumentOutOfRangeException();

            MyMath.XORInternal(_value, _iv);

            int oldValue = BitConverter.ToInt32(_value);
            int newValue = oldValue / value;

            SetValue(newValue);
        }

        #region interfaces

        public void Dispose()
        {
            if (_disposed == true)
                throw new DisposedException();

            MyMath.ResetMany(_value, _iv);
            _valueTracker.Cancel();

            _disposed = true;
        }

        public override bool Equals(object obj) =>
            obj is not null && Equals(obj as SafeInt) == true;

        public bool Equals(SafeInt other) =>
            GetValue() == other.GetValue();

        public override int GetHashCode() =>
            _value.GetHashCode() ^ _iv.GetHashCode();

        public int CompareTo(SafeInt other)
        {
            if (other is null)
                throw new ArgumentNullException();

            if (GetValue() < other.GetValue()) return -1;
            if (GetValue() > other.GetValue()) return 1;

            return 0;
        }

        public int Compare(SafeInt x, SafeInt y)
        {
            if (x is null || y is null)
                throw new ArgumentNullException();

            if (x.GetValue() < y.GetValue()) return -1;
            if (x.GetValue() > y.GetValue()) return 1;

            return 0;
        }

        public IDisposable Subscribe(IObserver<int> observer) =>
            _valueTracker.Subscribe(observer);

        #endregion
    }
}