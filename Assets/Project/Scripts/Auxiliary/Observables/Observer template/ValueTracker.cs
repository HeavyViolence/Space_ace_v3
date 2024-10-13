using System;
using System.Collections.Generic;

namespace SpaceAce.Auxiliary.Observables
{
    public sealed class ValueTracker<T> : IObservable<T>
    {
        private readonly HashSet<IObserver<T>> _observers = new();

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer is null)
            {
                throw new ArgumentNullException();
            }

            if (_observers.Contains(observer) == false)
            {
                _observers.Add(observer);
            }

            return new ValueObserverDisposer<T>(_observers, observer);
        }

        public void Track(T value)
        {
            if (value is null)
            {
                ArgumentNullException ex = new();

                foreach (var observer in _observers)
                {
                    observer.OnError(ex);
                }

                return;
            }

            foreach (var observer in _observers)
            {
                observer.OnNext(value);
            }
        }

        public void Cancel()
        {
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
            }
        }
    }
}