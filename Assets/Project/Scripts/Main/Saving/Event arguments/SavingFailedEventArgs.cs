using System;

namespace SpaceAce.Main.Saving
{
    public sealed class SavingFailedEventArgs : EventArgs
    {
        public string SavedDataName { get; }
        public Exception Error { get; }

        public SavingFailedEventArgs(string savedDataName, Exception error)
        {
            SavedDataName = savedDataName;
            Error = error ?? throw new ArgumentNullException();
        }
    }
}