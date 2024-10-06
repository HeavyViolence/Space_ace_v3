using System;

namespace SpaceAce.Auxiliary.Observables
{
    public sealed class ErrorOccurredEventArgs : EventArgs
    {
        public Exception Error { get; }

        public ErrorOccurredEventArgs(Exception error)
        {
            Error = error ?? throw new ArgumentNullException();
        }
    }
}