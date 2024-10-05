using System;

namespace SpaceAce.Main.Saving
{
    public sealed class StateSavedEventArgs : EventArgs
    {
        public string SavedDataName { get; }

        public StateSavedEventArgs(string savedDataName)
        {
            SavedDataName = savedDataName;
        }
    }
}