using System;
using System.Collections.Generic;

namespace SpaceAce.Auxiliary.Observables
{
    public sealed class ValueObserverDisposer<T> : IDisposable
    {
        private readonly HashSet<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        public ValueObserverDisposer(IEnumerable<IObserver<T>> observers, IObserver<T> observer)
        {
            if (observers is null)
                throw new ArgumentNullException();

            _observers = new(observers);
            _observer = observer ?? throw new ArgumentNullException();
        }

        public void Dispose()
        {
            if (_observer is not null && _observers.Contains(_observer) == true)
                _observers.Remove(_observer);
        }
    }
}