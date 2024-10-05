using System;

namespace SpaceAce.Main.GameStates
{
    public sealed class LevelLoadingStartedEventArgs : EventArgs
    {
        public int Level { get; }
        public float LoadDelay { get; }

        public LevelLoadingStartedEventArgs(int level, float loadDelay)
        {
            if (level < 1)
                throw new ArgumentOutOfRangeException();

            Level = level;

            if (loadDelay <= 0f)
                throw new ArgumentOutOfRangeException();

            LoadDelay = loadDelay;
        }
    }
}