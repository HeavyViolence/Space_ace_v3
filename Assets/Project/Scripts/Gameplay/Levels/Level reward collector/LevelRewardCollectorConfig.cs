using System;

using UnityEngine;

namespace SpaceAce.Gameplay.Levels
{
    [CreateAssetMenu(fileName = "Level reward collector config",
                     menuName = "Space ace/Configs/Levels/Level reward collector config")]
    public sealed class LevelRewardCollectorConfig : ScriptableObject
    {
        #region credits reward

        private const float MinFirstLevelCreditsReward = 0f;
        private const float MaxFirstLevelCreditsReward = 1_000f;

        private const float MinNextLevelCreditsRewardIncrease = 0f;
        private const float MaxNextLevelCreditsRewardIncrease = 1_000f;

        [SerializeField, Range(MinFirstLevelCreditsReward, MaxFirstLevelCreditsReward)]
        private float _firstLevelCreditsReward = MinFirstLevelCreditsReward;

        [SerializeField, Range(MinNextLevelCreditsRewardIncrease, MaxNextLevelCreditsRewardIncrease)]
        private float _nextLevelCreditsRewardIncrease = MinNextLevelCreditsRewardIncrease;

        public float GetCreditsReward(int level)
        {
            if (level <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            float reward = _firstLevelCreditsReward + _nextLevelCreditsRewardIncrease * (level - 1);
            return reward;
        }

        #endregion

        #region experience reward

        private const float MinFirstLevelExperienceReward = 0f;
        private const float MaxFirstLevelExperienceReward = 10_000f;

        private const float MinNextLevelExperienceRewardFactor = 1f;
        private const float MaxNextLevelExperienceRewardFactor = 2f;

        [SerializeField, Range(MinFirstLevelExperienceReward, MaxFirstLevelExperienceReward), Space]
        private float _firstLevelExperienceReward = MinFirstLevelExperienceReward;

        [SerializeField, Range(MinNextLevelExperienceRewardFactor, MaxNextLevelExperienceRewardFactor)]
        private float _nextLevelExperienceRewardFactor = MinNextLevelExperienceRewardFactor;

        public float GetExperienceReward(int level)
        {
            if (level <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            float reward = _firstLevelExperienceReward * Mathf.Pow(_nextLevelExperienceRewardFactor, level - 1);
            return reward;
        }

        #endregion
    }
}