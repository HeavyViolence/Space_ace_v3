using System;

namespace SpaceAce.Auxiliary.Observables
{
    public sealed class ValueChangedEventArgs<T> : EventArgs
    {
        public T Value { get; }

        public ValueChangedEventArgs(T value)
        {
            Value = value ?? throw new ArgumentNullException();
        }
    }
}