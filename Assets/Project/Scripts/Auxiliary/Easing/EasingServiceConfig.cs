using UnityEngine;

namespace SpaceAce.Auxiliary.Easing
{
    [CreateAssetMenu(fileName = "Easing service config",
                     menuName = "Space ace/Configs/Auxiliary/Easing service config")]
    public sealed class EasingServiceConfig : ScriptableObject
    {
        [SerializeField]
        private AnimationCurve _riseSlow;

        [SerializeField]
        private AnimationCurve _riseFast;

        [SerializeField]
        private AnimationCurve _riseSmooth;

        [SerializeField]
        private AnimationCurve _riseWavy;

        [SerializeField]
        private AnimationCurve _bell;

        [SerializeField]
        private AnimationCurve _bellSmooth;

        [SerializeField]
        private AnimationCurve _flatFastInOut;

        [SerializeField]
        private AnimationCurve _flatSmoothInOut;

        public float GetEasedTime(float t, EasingMode mode)
        {
            return mode switch
            {
                EasingMode.RiseSlow => _riseSlow.Evaluate(t),
                EasingMode.RiseFast => _riseFast.Evaluate(t),
                EasingMode.RiseSmooth => _riseSmooth.Evaluate(t),
                EasingMode.RiseWavy => _riseWavy.Evaluate(t),
                EasingMode.Bell => _bell.Evaluate(t),
                EasingMode.BellSmooth => _bellSmooth.Evaluate(t),
                EasingMode.FlatFastInOut => _flatFastInOut.Evaluate(t),
                EasingMode.FlatSmoothInOut => _flatSmoothInOut.Evaluate(t),
                _ => 0f,
            };
        }
    }
}