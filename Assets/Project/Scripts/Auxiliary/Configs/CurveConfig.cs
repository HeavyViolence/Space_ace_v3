using UnityEngine;

namespace SpaceAce.Auxiliary.Configs
{
    [CreateAssetMenu(fileName = "Curve config",
                     menuName = "Space ace/Configs/Value configs/Curve config")]
    public sealed class CurveConfig : ScriptableObject
    {
        [SerializeField]
        private AnimationCurve _curve;

        public AnimationCurve Curve => _curve;

        public float Evaluate(float time) => _curve.Evaluate(time);
    }
}