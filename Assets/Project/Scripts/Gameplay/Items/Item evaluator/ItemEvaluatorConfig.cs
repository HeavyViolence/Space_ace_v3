using NaughtyAttributes;

using SpaceAce.Auxiliary.Configs;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SpaceAce.Gameplay.Items
{
    [CreateAssetMenu(fileName = "Item evaluator config",
                     menuName = "Space Ace/Gameplay/Items/Related/Item evaluator config")]
    public sealed class ItemEvaluatorConfig : ScriptableObject
    {
        [SerializeField, Tooltip("Item spawn probability from the highest tier to the lowest")]
        private CurveConfig _probability;

        [SerializeField, Tooltip("Color codes for each tier from the highest to the lowest"), Space]
        private List<Color32> _tierColorCodes;

        private Dictionary<PowerClass, Color32> _colorCodes;
        private Dictionary<PowerClass, Vector2> _probabilities;

        public Dictionary<PowerClass, PowerClassData> Data { get; private set; }

        private void OnEnable()
        {
            if (_tierColorCodes.Count == 0 || _probability == null)
            {
                return;
            }

            _colorCodes = GetColorCodes();
            _probabilities = GetProbabilities();

            Data = new();

            for (int i = 0; i < Data.Count; i++)
            {
                PowerClass tier = (PowerClass)i;
                Color32 colorCode = _colorCodes[tier];
                Vector2 probability = _probabilities[tier];

                PowerClassData entry = new(colorCode, probability);
                Data.Add(tier, entry);
            }
        }

        private Dictionary<PowerClass, Color32> GetColorCodes()
        {
            PowerClass[] tiers = Enum.GetValues(typeof(PowerClass)).Cast<PowerClass>().ToArray();

            if (_tierColorCodes.Count != tiers.Length)
            {
                throw new Exception("Color codes amount doesn't correspond to tiers amount!");
            }

            Dictionary<PowerClass, Color32> codes = new(tiers.Length);

            for (int i = 0; i < tiers.Length; i++)
            {
                PowerClass tier = tiers[i];
                Color32 code = _tierColorCodes[i];

                codes.Add(tier, code);
            }

            return codes;
        }

        private Dictionary<PowerClass, Vector2> GetProbabilities()
        {
            PowerClass[] tiers = Enum.GetValues(typeof(PowerClass)).Cast<PowerClass>().ToArray();
            Dictionary<PowerClass, Vector2> probabilities = new(tiers.Length);

            for (int i = 0; i < tiers.Length; i++)
            {
                float startEvaluator = (float)i / tiers.Length;
                float endEvaluator = (float)(i + 1) / tiers.Length;

                float lowestProbability = _probability.Evaluate(startEvaluator);
                float highestProbability = _probability.Evaluate(endEvaluator);

                PowerClass tier = tiers[i];
                Vector2 probability = new(lowestProbability, highestProbability);

                probabilities.Add(tier, probability);
            }

            return probabilities;
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

        #endregion
    }
}