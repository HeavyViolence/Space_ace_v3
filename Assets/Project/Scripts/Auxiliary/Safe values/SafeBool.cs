using SpaceAce.Auxiliary.Exceptions;
using SpaceAce.Auxiliary.Observables;

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace SpaceAce.Auxiliary.SafeValues
{
    public sealed class SafeBool : IDisposable,
                                   IEquatable<SafeBool>,
                                   IComparable<SafeBool>,
                                   IComparer<SafeBool>,
                                   IObservable<bool>
    {
        private const int BytesPerBool = 1;

        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

        private readonly byte[] _value;
        private readonly byte[] _iv = new byte[BytesPerBool];

        private readonly ValueTracker<bool> _valueTracker = new();

        private bool _disposed = false;

        public SafeBool(bool value = false)
        {
            _value = BitConverter.GetBytes(value);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);
        }

        public bool Get()
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

            try
            {
                MyMath.XORInternal(_value, _iv);
                return BitConverter.ToBoolean(_value, 0);
            }
            finally
            {
                _rng.GetBytes(_iv);
                MyMath.XORInternal(_value, _iv);
            }
        }

        public void Set(bool value)
        {
            if (_disposed == true)
            {
                throw new DisposedException();
            }

            MyMath.XORInternal(_value, _iv);
            bool currentValue = BitConverter.ToBoolean(_value);

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
            obj is not null && Equals(obj as SafeBool) == true;

        public bool Equals(SafeBool other) =>
            other is not null && Get().Equals(other.Get()) == true;

        public override int GetHashCode() =>
            HashCode.Combine(_value, _iv);

        public int CompareTo(SafeBool other)
        {
            if (other is null)
            {
                throw new ArgumentNullException();
            }

            return Get().CompareTo(other.Get());
        }

        public int Compare(SafeBool x, SafeBool y)
        {
            if (x is null || y is null)
            {
                throw new ArgumentNullException();
            }

            return x.Get().CompareTo(y.Get());
        }

        public IDisposable Subscribe(IObserver<bool> observer) =>
            _valueTracker.Subscribe(observer);

        #endregion
    }
}