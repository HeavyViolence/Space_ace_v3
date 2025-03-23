using System;

namespace SpaceAce.Auxiliary.Observables
{
    public sealed class ValueReporter<T> : IObserver<T>
    {
        public event Action<T> ValueChanged;
        public event Action<Exception> ErrorOccurred;
        public event Action ReportsCompleted;

        private IDisposable _disposer;

        public void Subscribe(IObservable<T> source)
        {
            _disposer = source is null ? throw new ArgumentNullException()
                                       : source.Subscribe(this);
        }

        public void Unsubscribe() =>
            _disposer?.Dispose();

        public void OnNext(T value) =>
            ValueChanged?.Invoke(value);

        public void OnError(Exception error) =>
            ErrorOccurred?.Invoke(error);

        public void OnCompleted()
        {
            Unsubscribe();
            ReportsCompleted?.Invoke();
        }
    }
}