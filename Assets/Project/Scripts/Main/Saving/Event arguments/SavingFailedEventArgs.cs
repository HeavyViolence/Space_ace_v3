using System;

namespace SpaceAce.Main.Saving
{
    public sealed class SavingFailedEventArgs : EventArgs
    {
        public string SavedDataName { get; }
        public Exception Exception { get; }

        public SavingFailedEventArgs(string savedDataName, Exception exception)
        {
            SavedDataName = savedDataName;
            Exception = exception ?? throw new ArgumentNullException();
        }
    }
}