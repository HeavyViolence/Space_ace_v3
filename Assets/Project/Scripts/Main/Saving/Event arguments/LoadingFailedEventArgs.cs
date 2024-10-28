using System;

namespace SpaceAce.Main.Saving
{
    public sealed class LoadingFailedEventArgs : EventArgs
    {
        public string SavedDataName { get; }
        public Exception Error { get; }

        public LoadingFailedEventArgs(string savedDataName, Exception error)
        {
            SavedDataName = savedDataName;
            Error = error ?? throw new ArgumentNullException();
        }
    }
}