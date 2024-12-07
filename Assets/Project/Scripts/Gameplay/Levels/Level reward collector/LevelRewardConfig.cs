using SpaceAce.Gameplay.Items;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace SpaceAce.Gameplay.Levels
{
    [CreateAssetMenu(fileName = "Level reward config",
                     menuName = "Space ace/Configs/Levels/Level reward config")]
    public sealed class LevelRewardConfig : ScriptableObject
    {
        #region credits reward

        private const float MinFirstLevelCreditsReward = 0f;
        private const float MaxFirstLevelCreditsReward = 2_000f;

        private const float MinNextLevelCreditsRewardIncrease = 0f;
        private const float MaxNextLevelCreditsRewardIncrease = 2_000f;

        [SerializeField, Range(MinFirstLevelCreditsReward, MaxFirstLevelCreditsReward)]
        private float _firstLevelCreditsReward = MinFirstLevelCreditsReward;

        [SerializeField, Range(MinNextLevelCreditsRewardIncrease, MaxNextLevelCreditsRewardIncrease)]
        private float _nextLevelCreditsRewardIncrease = MinNextLevelCreditsRewardIncrease;

        private float GetCreditsReward(int level) =>
            _firstLevelCreditsReward + _nextLevelCreditsRewardIncrease * (level - 1);

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

        private float GetExperienceReward(int level) =>
            _firstLevelExperienceReward * Mathf.Pow(_nextLevelExperienceRewardFactor, level - 1);

        #endregion

        #region items reward

        private const int MinItems = 1;
        private const int MaxItems = 10;

        [SerializeField, Range(MinItems, MaxItems), Space]
        private int _itemsToBeAwarded = MinItems;

        [SerializeField, Range(MinItems, MaxItems)]
        private int _ammoToBeAwarded = MinItems;

        [SerializeField, Space]
        private List<Tier> _allowedTiers;

        public int ItemsToBeAwarded => _itemsToBeAwarded;
        public int AmmoToBeAwarded => _ammoToBeAwarded;

        public IEnumerable<Tier> AllowedTiers => _allowedTiers;

        #endregion

        public LevelReward GetReward(int level, float creditsSupplement = 0f, float experienceSupplement = 0f)
        {
            float credits = GetCreditsReward(level) + creditsSupplement;
            float experience = GetExperienceReward(level) + experienceSupplement;

            return new(credits, experience);
        }
    }
}