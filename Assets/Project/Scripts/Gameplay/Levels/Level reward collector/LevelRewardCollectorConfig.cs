using UnityEngine;

namespace SpaceAce.Gameplay.Levels
{
    [CreateAssetMenu(fileName = "Level reward collector config",
                     menuName = "Space ace/Configs/Levels/Level reward collector config")]
    public sealed class LevelRewardCollectorConfig : ScriptableObject
    {
        public const float LevelMasteryThreshold = 0.33f;
        public const float LevelExcellenceThreshold = 0.66f;

        [SerializeField]
        private LevelRewardConfig _levelCompletionReward;

        [SerializeField]
        private LevelRewardConfig _levelMasteryReward;

        [SerializeField]
        private LevelRewardConfig _levelExcellenceReward;

        public LevelRewardBundle GetReward(int level, float creditsSupplement = 0f, float experienceSupplement = 0f)
        {
            LevelReward completionReward = _levelCompletionReward.GetReward(level, creditsSupplement, experienceSupplement);
            LevelReward masteryReward = _levelMasteryReward.GetReward(level, creditsSupplement, experienceSupplement);
            LevelReward excellenceReward = _levelExcellenceReward.GetReward(level, creditsSupplement, experienceSupplement);

            return new(completionReward, masteryReward, excellenceReward);
        }
    }
}