using System;

namespace SpaceAce.Auxiliary.EventArguments
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