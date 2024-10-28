using SpaceAce.Auxiliary.Exceptions;
using SpaceAce.Auxiliary.Observables;

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SpaceAce.Auxiliary.SafeValues
{
    public sealed class SafeFloat : IDisposable,
                                    IEquatable<SafeFloat>,
                                    IComparable<SafeFloat>,
                                    IComparer<SafeFloat>,
                                    IObservable<float>
    {
        private const int BytesPerFloat = 4;

        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        private readonly byte[] _value;
        private readonly byte[] _iv = new byte[BytesPerFloat];

        private readonly ValueTracker<float> _valueTracker = new();

        private bool _disposed = false;

        public SafeFloat(float value = 0f)
        {
            _value = BitConverter.GetBytes(value);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);
        }

        public float Get()
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

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

        public void Set(float value)
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

            MyMath.XORInternal(_value, _iv);
            float currentValue = BitConverter.ToSingle(_value);

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

        public void Add(float value)
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

            if (value == 0f)
            {
                return;
            }

            MyMath.XORInternal(_value, _iv);

            float oldValue = BitConverter.ToSingle(_value);
            float newValue = oldValue + value;

            BitConverter.GetBytes(newValue).CopyTo(_value, 0);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);

            _valueTracker.Track(newValue);
        }

        public void Subtract(float value) => Add(-1f * value);

        public void MultiplyBy(float value)
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

            if (value == 1f)
            {
                return;
            }

            MyMath.XORInternal(_value, _iv);

            float oldValue = BitConverter.ToSingle(_value);
            float newValue = oldValue * value;

            BitConverter.GetBytes(newValue).CopyTo(_value, 0);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);

            _valueTracker.Track(newValue);
        }

        public void DivideBy(float value)
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

            if (value == 0f)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (value == 1f)
            {
                return;
            }

            MyMath.XORInternal(_value, _iv);

            float oldValue = BitConverter.ToSingle(_value);
            float newValue = oldValue / value;

            BitConverter.GetBytes(newValue).CopyTo(_value, 0);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);

            _valueTracker.Track(newValue);
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
            obj is not null && Equals(obj as SafeFloat) == true;

        public bool Equals(SafeFloat other) =>
            other is not null && Get().Equals(other.Get()) == true;

        public override int GetHashCode() =>
            _value.GetHashCode() ^ _iv.GetHashCode();

        public int CompareTo(SafeFloat other)
        {
            if (other is null)
            {
                throw new ArgumentNullException();
            }

            return Get().CompareTo(other.Get());
        }

        public int Compare(SafeFloat x, SafeFloat y)
        {
            if (x is null || y is null)
            {
                throw new ArgumentNullException();
            }

            return x.Get().CompareTo(y.Get());
        }

        public IDisposable Subscribe(IObserver<float> observer) =>
            _valueTracker.Subscribe(observer);

        #endregion
    }
}