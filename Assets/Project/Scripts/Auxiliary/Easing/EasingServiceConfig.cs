using UnityEngine;

namespace SpaceAce.Auxiliary.Easing
{
    [CreateAssetMenu(fileName = "Easing service config",
                     menuName = "Space ace/Configs/Auxiliary/Easing service config")]
    public sealed class EasingServiceConfig : ScriptableObject
    {
        [SerializeField]
        private AnimationCurve _slow;

        [SerializeField]
        private AnimationCurve _fast;

        [SerializeField]
        private AnimationCurve _smooth;

        [SerializeField]
        private AnimationCurve _wavy;

        public float GetEasedTime(float t, EasingMode mode)
        {
            return mode switch
            {
                EasingMode.Slow => _slow.Evaluate(t),
                EasingMode.Fast => _fast.Evaluate(t),
                EasingMode.Smooth => _smooth.Evaluate(t),
                EasingMode.Vawy => _wavy.Evaluate(t),
                _ => 0f,
            };
        }
    }
}