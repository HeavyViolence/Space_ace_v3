using System;

namespace SpaceAce.Main.GameStates
{
    public sealed class LevelLoadedEventArgs : EventArgs
    {
        public int Level { get; }

        public LevelLoadedEventArgs(int level)
        {
            if (level < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
                
            Level = level;
        }
    }
}