using System;

namespace SpaceAce.Auxiliary.Observables
{
    public sealed class ObservableValue<T> : IObservable<T>, IDisposable
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

        public IDisposable Subscribe(IObserver<T> observer) =>
            _valueTracker.Subscribe(observer);

        public void Dispose()
        {
            _value = default;
            _valueTracker.Cancel();
        }
    }
}