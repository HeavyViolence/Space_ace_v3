using System;

namespace SpaceAce.Gameplay.Levels
{
    public sealed class LevelCompleter
    {
        public event EventHandler<LevelEventArgs> LevelCompleted, LevelFailed, LevelConcluded;
    }
}