using System;

namespace SpaceAce.Gameplay.Levels
{
    public sealed class LevelEventArgs : EventArgs
    {
        public int Level { get; }

        public LevelEventArgs(int level)
        {
            Level = level;
        }
    }
}