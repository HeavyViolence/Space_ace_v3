using System;

namespace SpaceAce.Auxiliary.Observables
{
    public sealed class ValueReporter<T> : IObserver<T>
    {
        public event EventHandler<ValueChangedEventArgs<T>> ValueChanged;
        public event EventHandler<ErrorOccurredEventArgs> ErrorOccurred;
        public event EventHandler ReportsCompleted;

        private IDisposable _disposer;

        public void Subscribe(IObservable<T> source)
        {
            if (source is null)
                throw new ArgumentNullException();

            _disposer = source.Subscribe(this);
        }

        public void Unsubscribe() =>
            _disposer?.Dispose();

        public void OnNext(T value) =>
            ValueChanged?.Invoke(this, new(value));

        public void OnError(Exception error) =>
            ErrorOccurred?.Invoke(this, new(error));

        public void OnCompleted()
        {
            Unsubscribe();
            ReportsCompleted?.Invoke(this, EventArgs.Empty);
        }
    }
}