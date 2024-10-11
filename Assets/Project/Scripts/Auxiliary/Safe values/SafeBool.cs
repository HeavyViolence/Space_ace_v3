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

        private byte[] _value;
        private readonly byte[] _iv = new byte[BytesPerBool];
        private bool _disposed = false;

        private readonly ValueTracker<bool> _valueTracker = new();

        public SafeBool(bool value = false)
        {
            _value = BitConverter.GetBytes(value);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);
        }

        public bool Get()
        {
            if (_disposed == true)
                throw new DisposedException();

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
                throw new DisposedException();

            _value = BitConverter.GetBytes(value);

            _rng.GetBytes(_iv);
            MyMath.XORInternal(_value, _iv);

            _valueTracker.Track(value);
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
            obj is not null && Equals(obj as SafeBool) == true;

        public bool Equals(SafeBool other) =>
            other is not null && Get() == other.Get();

        public override int GetHashCode() =>
            _value.GetHashCode() ^ _iv.GetHashCode();

        public int CompareTo(SafeBool other)
        {
            if (other is null)
                throw new ArgumentNullException();

            return Get().CompareTo(other.Get());
        }

        public int Compare(SafeBool x, SafeBool y)
        {
            if (x is null || y is null)
                throw new ArgumentNullException();

            return x.Get().CompareTo(y.Get());
        }

        public IDisposable Subscribe(IObserver<bool> observer) =>
            _valueTracker.Subscribe(observer);

        #endregion
    }
}