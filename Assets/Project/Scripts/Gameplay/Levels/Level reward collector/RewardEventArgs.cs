using System;

namespace SpaceAce.Gameplay.Levels
{
    public sealed class RewardEventArgs : EventArgs
    {
        public float CreditsReward { get; }
        public float ExperienceReward { get; }

        public RewardEventArgs(float credits, float experience)
        {
            CreditsReward = credits;
            ExperienceReward = experience;
        }
    }
}