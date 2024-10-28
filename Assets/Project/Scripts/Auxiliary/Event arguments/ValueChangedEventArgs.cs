using System;

namespace SpaceAce.Auxiliary.EventArguments
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