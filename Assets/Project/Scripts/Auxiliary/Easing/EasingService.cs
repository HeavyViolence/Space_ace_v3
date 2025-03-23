using System;

using UnityEngine;

using VContainer;

namespace SpaceAce.Auxiliary.Easing
{
    public sealed class EasingService
    {
        private readonly EasingServiceConfig _config;

        [Inject]
        public EasingService(EasingServiceConfig config)
        {
            _config = config == null ? throw new ArgumentNullException() : config;
        }

        public float Ease(float a, float b, float t, EasingMode mode)
        {
            float tEased = _config.GetEasedTime(t, mode);
            float value = Mathf.Lerp(a, b, tEased);

            return value;
        }

        public float Ease(Vector2 range, float t, EasingMode mode)
        {
            float tEased = _config.GetEasedTime(t, mode);
            float value = Mathf.Lerp(range.x, range.y, tEased);

            return value;
        }

        public int Ease(int a, int b, float t, EasingMode mode)
        {
            float tEased = _config.GetEasedTime(t, mode);
            int value = Mathf.FloorToInt(a + (b - a) * tEased);

            return value;
        }

        public int Ease(Vector2Int range, float t, EasingMode mode)
        {
            float tEased = _config.GetEasedTime(t, mode);
            int value = Mathf.FloorToInt(range.x + (range.y - range.x) * tEased);

            return value;
        }
    }
}