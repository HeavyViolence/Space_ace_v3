using System;

namespace SpaceAce.Gameplay.Levels
{
    public sealed record LevelRewardBundle
    {
        public LevelReward CompletionReward { get; }
        public LevelReward MasteryReward { get; }
        public LevelReward ExcellenceReward { get; }

        public LevelRewardBundle(LevelReward completionReward,
                                 LevelReward masteryReward,
                                 LevelReward excellenceReward)
        {
            CompletionReward = completionReward ?? throw new ArgumentNullException();
            MasteryReward = masteryReward ?? throw new ArgumentNullException();
            ExcellenceReward = excellenceReward ?? throw new ArgumentNullException();
        }
    }
}