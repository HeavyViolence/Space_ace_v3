using NaughtyAttributes;

using SpaceAce.Auxiliary.Configs;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SpaceAce.Gameplay.Items
{
    [CreateAssetMenu(fileName = "Item evaluator config",
                     menuName = "Space ace/Configs/Items/Related/Item evaluator config")]
    public sealed class ItemEvaluatorConfig : ScriptableObject
    {
        private const float DefaultGradeInfluence = 0.5f;

        private const float MinGrade = 0f;
        private const float MaxGrade = 2000f;

        private const float MinGradeGap = 0f;
        private const float MaxGradeGap = 1f;

        [SerializeField, Tooltip("Item spawn probability from the highest tier to the lowest")]
        private CurveConfig _probability;

        [SerializeField, Tooltip("Grade distribution from the highest tier to the lowest")]
        private CurveConfig _grade;

        [SerializeField, Tooltip("Item property influence factor from the highest grade to the lowest")]
        private CurveConfig _influence;

        [SerializeField, MinMaxSlider(MinGrade, MaxGrade), Space]
        private Vector2 _gradeRange = new(MinGrade, MaxGrade);

        [SerializeField, Range(MinGradeGap, MaxGradeGap), Tooltip("Grade gap between the tiers")]
        private float _gradeGap = MinGradeGap;

        [SerializeField, Tooltip("Color codes for each tier from the highest to the lowest"), Space]
        private List<Color32> _tierColorCodes;

        public Vector2 FullGradeRange => _gradeRange;
        public Dictionary<Tier, Color32> ColorCodes { get; private set; }
        public Dictionary<Tier, Vector2> Probabilities { get; private set; }
        public Dictionary<Tier, Vector2> GradeRanges { get; private set; }
        public Dictionary<Tier, Vector2> DirectGradeInfluence { get; private set; }
        public Dictionary<Tier, Vector2> InvertedGradeInfluence { get; private set; }
        public Dictionary<Tier, TierData> Data { get; private set; }

        private Dictionary<Tier, Vector2> GetProbabilities()
        {
            Tier[] tiers = Enum.GetValues(typeof(Tier)).Cast<Tier>().ToArray();
            Dictionary<Tier, Vector2> probabilities = new(tiers.Length);

            for (int i = 0; i < tiers.Length; i++)
            {
                float startEvaluator = (float)i / tiers.Length;
                float endEvaluator = (float)(i + 1) / tiers.Length;

                float lowestProbability = _probability.Evaluate(startEvaluator);
                float highestProbability = _probability.Evaluate(endEvaluator);

                Tier tier = tiers[i];
                Vector2 probability = new(lowestProbability, highestProbability);

                probabilities.Add(tier, probability);
            }

            return probabilities;
        }

        private Dictionary<Tier, Vector2> GetGradeInfluence(GradeInfluence influence)
        {
            Tier[] tiers = Enum.GetValues(typeof(Tier)).Cast<Tier>().ToArray();
            Dictionary<Tier, Vector2> ranges = new(tiers.Length);

            float lowestInfluence, highestInfluence;

            for (int i = 0; i < tiers.Length; i++)
            {
                float startEvaluator = (float)i / tiers.Length;
                float endEvaluator = (float)(i + 1) / tiers.Length;

                switch (influence)
                {
                    case GradeInfluence.Direct:
                        {
                            lowestInfluence = _influence.Evaluate(endEvaluator);
                            highestInfluence = _influence.Evaluate(startEvaluator);

                            break;
                        }
                    case GradeInfluence.Inverted:
                        {
                            lowestInfluence = 1f - _influence.Evaluate(startEvaluator);
                            highestInfluence = 1f - _influence.Evaluate(endEvaluator);

                            break;
                        }
                    case GradeInfluence.None:
                        {
                            lowestInfluence = DefaultGradeInfluence;
                            highestInfluence = DefaultGradeInfluence;

                            break;
                        }
                    default:
                        {
                            lowestInfluence = DefaultGradeInfluence;
                            highestInfluence = DefaultGradeInfluence;

                            break;
                        }
                }

                Tier tier = tiers[i];
                Vector2 range = new(lowestInfluence, highestInfluence);

                ranges.Add(tier, range);
            }

            return ranges;
        }

        private Dictionary<Tier, Vector2> GetGradeRanges()
        {
            Tier[] tiers = Enum.GetValues(typeof(Tier)).Cast<Tier>().ToArray();
            Dictionary<Tier, Vector2> ranges = new(tiers.Length);

            for (int i = 0; i < tiers.Length; i++)
            {
                float startEvaluator = (float)i / tiers.Length;
                float endEvaluator = (float)(i + 1) / tiers.Length;

                float lowestGarde = Mathf.Lerp(_gradeRange.x, _gradeRange.y, _grade.Evaluate(endEvaluator));
                float highestGrade = i == 0 ? Mathf.Lerp(_gradeRange.x, _gradeRange.y, _grade.Evaluate(startEvaluator))
                                            : Mathf.Lerp(_gradeRange.x, _gradeRange.y, _grade.Evaluate(startEvaluator)) - _gradeGap;

                Tier tier = tiers[i];
                Vector2 gradeRange = new(lowestGarde, highestGrade);

                ranges.Add(tier, gradeRange);
            }

            return ranges;
        }

        private Dictionary<Tier, Color32> GetColorCodes()
        {
            Tier[] tiers = Enum.GetValues(typeof(Tier)).Cast<Tier>().ToArray();

            if (_tierColorCodes.Count != tiers.Length)
            {
                throw new Exception("Color codes amount doesn't correspond to tiers amount!");
            }

            Dictionary<Tier, Color32> codes = new(tiers.Length);

            for (int i = 0; i < tiers.Length; i++)
            {
                Tier tier = tiers[i];
                Color32 code = _tierColorCodes[i];

                codes.Add(tier, code);
            }

            return codes;
        }

        private void OnEnable()
        {
            if (_tierColorCodes.Count == 0 || _probability == null ||
                _grade == null || _influence == null)
            {
                return;
            }

            ColorCodes = GetColorCodes();
            Probabilities = GetProbabilities();
            GradeRanges = GetGradeRanges();
            DirectGradeInfluence = GetGradeInfluence(GradeInfluence.Direct);
            InvertedGradeInfluence = GetGradeInfluence(GradeInfluence.Inverted);

            Data = new();

            for (int i = 0; i < Data.Count; i++)
            {
                Tier tier = (Tier)i;
                Color32 colorCode = ColorCodes[tier];
                Vector2 probability = Probabilities[tier];
                Vector2 grade = GradeRanges[tier];
                Vector2 directInfluence = DirectGradeInfluence[tier];
                Vector2 invertedInfluence = InvertedGradeInfluence[tier];

                TierData entry = new(colorCode, probability, grade, directInfluence, invertedInfluence);
                Data.Add(tier, entry);
            }
        }

        #region logs

        [Button("Log probability ranges")]
        private void LogProbabilities()
        {
            var probabilities = GetProbabilities();

            foreach (var entry in probabilities)
            {
                Debug.Log($"{entry.Key} -> {entry.Value.x:n2}-{entry.Value.y:n2}");
            }
        }

        [Button("Log grade ranges")]
        private void LogGradeRanges()
        {
            var gradeRanges = GetGradeRanges();

            foreach (var entry in gradeRanges)
            {
                Debug.Log($"{entry.Key} -> {entry.Value.x:n2}-{entry.Value.y:n2}");
            }
        }

        [Button("Log direct grade influence ranges")]
        private void LogDirectGradeInfluence()
        {
            var influence = GetGradeInfluence(GradeInfluence.Direct);

            foreach (var entry in influence)
            {
                Debug.Log($"{entry.Key} -> {entry.Value.x:n2}-{entry.Value.y:n2}");
            }
        }

        [Button("Log inverted grade influence ranges")]
        private void LogInvertedGradeInfluence()
        {
            var influence = GetGradeInfluence(GradeInfluence.Inverted);

            foreach (var entry in influence)
            {
                Debug.Log($"{entry.Key} -> {entry.Value.x:n2}-{entry.Value.y:n2}");
            }
        }

        #endregion
    }
}