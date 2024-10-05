using System;

namespace SpaceAce.Main.Saving
{
    public sealed class LoadingFailedEventArgs : EventArgs
    {
        public string SavedDataName { get; }
        public Exception Exception { get; }

        public LoadingFailedEventArgs(string savedDataName, Exception ex)
        {
            SavedDataName = savedDataName;
            Exception = ex ?? throw new ArgumentNullException();
        }
    }
}