using System;

namespace SpaceAce.UI
{
    public sealed class LevelButtonEventArgs : EventArgs
    {
        public int Level { get; }

        public LevelButtonEventArgs(int level)
        {
            Level = level;
        }
    }
}