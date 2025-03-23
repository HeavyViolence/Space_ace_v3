using UnityEngine;

namespace SpaceAce.Gameplay.Items
{
    public readonly struct PowerClassData
    {
        public Color32 ColorCode { get; }
        public Vector2 Probability { get; }

        public PowerClassData(Color32 code, Vector2 probability)
        {
            ColorCode = code;
            Probability = probability;
        }
    }
}