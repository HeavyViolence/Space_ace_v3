using System;

using UnityEngine;

namespace SpaceAce.Auxiliary.Easing
{
    public sealed class EasingService
    {
        private readonly EasingServiceConfig _config;

        public EasingService(EasingServiceConfig config)
        {
            if (config == null)
                throw new ArgumentNullException();

            _config = config;
        }

        public float Ease(float min, float max, float t, EasingMode mode)
        {
            float tEased = _config.GetEasedTime(t, mode);
            return Mathf.Lerp(min, max, tEased);
        }
    }
}