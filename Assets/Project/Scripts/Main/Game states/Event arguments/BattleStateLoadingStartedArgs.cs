using System;

namespace SpaceAce.Main.GameStates
{
    public readonly struct BattleStateLoadingStartedArgs
    {
        public BattleDifficulty Difficulty { get; }
        public float LoadDelay { get; }

        public BattleStateLoadingStartedArgs(BattleDifficulty difficulty, float loadDelay)
        {
            Difficulty = difficulty;

            if (loadDelay <= 0f)
            {
                throw new ArgumentOutOfRangeException();
            }

            LoadDelay = loadDelay;
        }
    }
}