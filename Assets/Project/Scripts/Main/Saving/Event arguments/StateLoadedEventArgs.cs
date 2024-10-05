using System;

namespace SpaceAce.Main.Saving
{
    public sealed class StateLoadedEventArgs : EventArgs
    {
        public string SavedDataName { get; }

        public StateLoadedEventArgs(string savedDataName)
        {
            SavedDataName = savedDataName;
        }
    }
}