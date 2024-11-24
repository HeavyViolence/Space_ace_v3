using UnityEngine;

namespace SpaceAce.Gameplay.Items
{
    public readonly struct TierData
    {
        public Color32 ColorCode { get; }
        public Vector2 ProbabilityRange { get; }
        public Vector2 GradeRange { get; }
        public Vector2 DirectInfluenceRange { get; }
        public Vector2 InvertedInfluenceRange { get; }

        public TierData(Color32 code,
                        Vector2 probabilityRange,
                        Vector2 gradeRange,
                        Vector2 directInfluenceRange,
                        Vector2 invertedInfluenceRange)
        {
            ColorCode = code;
            ProbabilityRange = probabilityRange;
            GradeRange = gradeRange;
            DirectInfluenceRange = directInfluenceRange;
            InvertedInfluenceRange = invertedInfluenceRange;
        }
    }
}