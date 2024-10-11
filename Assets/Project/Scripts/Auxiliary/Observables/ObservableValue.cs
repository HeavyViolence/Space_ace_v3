using System;

namespace SpaceAce.Auxiliary.Observables
{
    public sealed class ObservableValue<T> : IDisposable, IObservable<T> where T : struct
    {
        private readonly ValueTracker<T> _valueTracker = new();

        private T _value;

        public T Value
        {
            get => _value;

            set
            {
                _value = value;
                _valueTracker.Track(value);
            }
        }

        public void Dispose()
        {
            _value = default;
            _valueTracker.Cancel();
        }

        public IDisposable Subscribe(IObserver<T> observer) =>
            _valueTracker.Subscribe(observer);
    }
}