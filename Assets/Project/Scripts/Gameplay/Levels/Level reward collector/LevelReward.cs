using System;

namespace SpaceAce.Gameplay.Levels
{
    public sealed record LevelReward
    {
        public static LevelReward Empty => new(0f, 0f);

        public float Credits { get; }
        public float Experience { get; }

        public LevelReward(float credits, float experience)
        {
            Credits = credits < 0f ? throw new ArgumentOutOfRangeException() : credits;
            Experience = experience < 0f ? throw new ArgumentOutOfRangeException() : experience;
        }
    }
}