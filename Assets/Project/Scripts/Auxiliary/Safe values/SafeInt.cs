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

        private readonly byte[] _value;
        private readonly byte[] _iv = new byte[BytesPerInt];

        private readonly ValueTracker<int> _valueTracker = new();

        private bool _disposed = false;

        public SafeInt(int value = 0)
        {
            _value = BitConverter.GetBytes(value);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);
        }

        public int Get()
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

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

        public void Set(int value)
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

            MyMath.XORInternal(_value, _iv);
            int currentValue = BitConverter.ToInt32(_value, 0);

            if (currentValue == value)
            {
                MyMath.XORInternal(_value, _iv);
            }
            else
            {
                BitConverter.GetBytes(value).CopyTo(_value, 0);

                _rng.GetBytes(_iv);
                MyMath.XORInternal(_value, _iv);

                _valueTracker.Track(value);
            }
        }

        public void Add(int value)
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

            if (value == 0)
            {
                return;
            }

            MyMath.XORInternal(_value, _iv);

            int oldValue = BitConverter.ToInt32(_value);
            int newValue = oldValue + value;

            BitConverter.GetBytes(newValue).CopyTo(_value, 0);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);

            _valueTracker.Track(newValue);
        }

        public void Subtract(int value) => Add(-1 * value);

        public void MultiplyBy(int value)
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

            if (value == 1)
            {
                return;
            }

            MyMath.XORInternal(_value, _iv);

            int oldValue = BitConverter.ToInt32(_value);
            int newValue = oldValue * value;

            BitConverter.GetBytes(newValue).CopyTo(_value, 0);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);
        }

        public void DivideBy(int value)
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

            if (value == 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (value == 1)
            {
                return;
            }

            MyMath.XORInternal(_value, _iv);

            int oldValue = BitConverter.ToInt32(_value);
            int newValue = oldValue / value;

            BitConverter.GetBytes(newValue).CopyTo(_value, 0);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);
        }

        #region interfaces

        public void Dispose()
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

            MyMath.ResetMany(_value, _iv);
            _valueTracker.Cancel();

            _disposed = true;
        }

        public override bool Equals(object obj) =>
            obj is not null && Equals(obj as SafeInt) == true;

        public bool Equals(SafeInt other) =>
            other is not null && Get().Equals(other.Get()) == true;

        public override int GetHashCode() =>
            HashCode.Combine(_value, _iv);

        public int CompareTo(SafeInt other)
        {
            if (other is null)
            {
                throw new ArgumentNullException();
            }

            return Get().CompareTo(other.Get());
        }

        public int Compare(SafeInt x, SafeInt y)
        {
            if (x is null || y is null)
            {
                throw new ArgumentNullException();
            }

            return x.Get().CompareTo(y.Get());
        }

        public IDisposable Subscribe(IObserver<int> observer) =>
            _valueTracker.Subscribe(observer);

        #endregion
    }
}