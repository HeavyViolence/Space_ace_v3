using SpaceAce.Auxiliary;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SpaceAce.Gameplay.Items
{
    public sealed class ItemEvaluator
    {
        private const float DefaultPropertyFactor = 0.5f;

        private readonly ItemEvaluatorConfig _config;

        public ItemEvaluator(ItemEvaluatorConfig config)
        {
            _config = config == null ? throw new ArgumentNullException() : config;
        }

        public Color32 GetColorCode(Tier tier) => _config.ColorCodes[tier];

        #region quality

        public Quality GetQuality(Tier tier, QualityType type)
        {
            float grade;

            switch (type)
            {
                case QualityType.Lowest:
                    {
                        grade = _config.GradeRanges[tier].x;
                        break;
                    }
                case QualityType.Highest:
                    {
                        grade = _config.GradeRanges[tier].y;
                        break;
                    }
                case QualityType.Average:
                    {
                        Vector2 range = _config.GradeRanges[tier];
                        grade = (range.x + range.y) / 2f;

                        break;
                    }
                case QualityType.Random:
                    {
                        Vector2 range = _config.GradeRanges[tier];
                        grade = Mathf.Lerp(range.x, range.y, MyMath.RandomUnit);

                        break;
                    }
                default:
                    {
                        grade = _config.GradeRanges[tier].x;
                        break;
                    }
            }

            return new(tier, grade);
        }

        public IEnumerable<Quality> GetQualities(int amount, QualityType type, IEnumerable<Tier> exclusions = null, int seed = 0)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            IEnumerable<Tier> allowedTiers = Enum.GetValues(typeof(Tier))
                                                 .Cast<Tier>()
                                                 .Except(exclusions ?? Enumerable.Empty<Tier>());

            float totalProbability = 0f;

            foreach (var tier in allowedTiers)
            {
                totalProbability += _config.Probabilities[tier].y;
            }

            Dictionary<Tier, float> normalizedAllowedTiers = new(allowedTiers.Count());

            foreach (var tier in allowedTiers)
            {
                float normalizedProbability = _config.Probabilities[tier].y / totalProbability;
                normalizedAllowedTiers.Add(tier, normalizedProbability);
            }

            System.Random random = seed == 0 ? new() : new(seed);
            List<Quality> result = new(amount);

            while (result.Count < amount)
            {
                float t = (float)random.NextDouble();
                float sum = 0f;

                foreach (var item in normalizedAllowedTiers)
                {
                    sum += item.Value;

                    if (sum >= t)
                    {
                        Tier tier = item.Key;
                        float grade;

                        switch (type)
                        {
                            case QualityType.Lowest:
                                {
                                    grade = _config.GradeRanges[tier].x;
                                    break;
                                }
                            case QualityType.Highest:
                                {
                                    grade = _config.GradeRanges[tier].y;
                                    break;
                                }
                            case QualityType.Average:
                                {
                                    Vector2 range = _config.GradeRanges[tier];
                                    grade = (range.x + range.y) / 2f;

                                    break;
                                }
                            case QualityType.Random:
                                {
                                    Vector2 range = _config.GradeRanges[tier];
                                    float evaluator = (float)random.NextDouble();
                                    grade = Mathf.Lerp(range.x, range.y, evaluator);

                                    break;
                                }
                            default:
                                {
                                    grade = _config.GradeRanges[tier].x;
                                    break;
                                }
                        }

                        Quality quality = new(tier, grade);
                        result.Add(quality);
                    }
                }
            }

            return result;
        }

        #endregion

        #region property evaluators

        public float Evaluate(Vector2 propertyRange, Quality quality, GradeInfluence influence)
        {
            if (propertyRange == Vector2.zero)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (quality is null)
            {
                throw new ArgumentNullException();
            }

            Vector2 gradeRange = _config.FullGradeRange;
            float evaluator = Mathf.InverseLerp(gradeRange.x, gradeRange.y, quality.Grade);
            float propertyFactor;

            switch (influence)
            {
                case GradeInfluence.Direct:
                    {
                        Vector2 influenceRange = _config.DirectGradeInfluence[quality.Tier];
                        propertyFactor = Mathf.Lerp(influenceRange.x, influenceRange.y, evaluator);

                        break;
                    }
                case GradeInfluence.Inverted:
                    {
                        Vector2 influenceRange = _config.InvertedGradeInfluence[quality.Tier];
                        propertyFactor = Mathf.Lerp(influenceRange.x, influenceRange.y, evaluator);

                        break;
                    }
                case GradeInfluence.None:
                    {
                        propertyFactor = DefaultPropertyFactor;
                        break;
                    }
                default:
                    {
                        propertyFactor = DefaultPropertyFactor;
                        break;
                    }
            }

            return Mathf.Lerp(propertyRange.x, propertyRange.y, propertyFactor);
        }

        public int Evaluate(Vector2Int propertyRange, Quality quality, GradeInfluence influence)
        {
            if (propertyRange == Vector2Int.zero)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (quality is null)
            {
                throw new ArgumentNullException();
            }

            Vector2 gradeRange = _config.FullGradeRange;
            float evaluator = Mathf.InverseLerp(gradeRange.x, gradeRange.y, quality.Grade);
            float propertyFactor;

            switch (influence)
            {
                case GradeInfluence.Direct:
                    {
                        Vector2 influenceRange = _config.DirectGradeInfluence[quality.Tier];
                        propertyFactor = Mathf.Lerp(influenceRange.x, influenceRange.y, evaluator);

                        break;
                    }
                case GradeInfluence.Inverted:
                    {
                        Vector2 influenceRange = _config.InvertedGradeInfluence[quality.Tier];
                        propertyFactor = Mathf.Lerp(influenceRange.x, influenceRange.y, evaluator);

                        break;
                    }
                case GradeInfluence.None:
                    {
                        propertyFactor = DefaultPropertyFactor;
                        break;
                    }
                default:
                    {
                        propertyFactor = DefaultPropertyFactor;
                        break;
                    }
            }

            float rawProperty = Mathf.Lerp(propertyRange.x, propertyRange.y, propertyFactor);
            return Mathf.CeilToInt(rawProperty);
        }

        #endregion
    }
}