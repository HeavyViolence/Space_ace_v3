using NaughtyAttributes;

using SpaceAce.Auxiliary.Configs;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SpaceAce.Gameplay.Items
{
    [CreateAssetMenu(fileName = "Item float property config",
                     menuName = "Space Ace/Gameplay/Items/Related/Item float property config")]
    public sealed class ItemFloatPropertyConfig : ScriptableObject
    {
        #region value range

        private static readonly float s_powerClassCount = Enum.GetValues(typeof(PowerClass)).Length - 1;

        public const float MinValue = 0f;
        public const float MaxValue = 9_999f;

        [SerializeField, MinMaxSlider(MinValue, MaxValue)]
        private Vector2 _value = new(MinValue, MaxValue);

        [SerializeField]
        private RangeEvaluation _evaluation = RangeEvaluation.LeftToRight;

        [SerializeField]
        private bool _round = false;

        public float Evaluate(PowerClass @class)
        {
            float t = (float)@class / s_powerClassCount;
            float interpolator = Interpolate(t);
            float value;

            switch (_evaluation)
            {
                case RangeEvaluation.LeftToRight:
                    {
                        value = Mathf.Lerp(_value.x, _value.y, interpolator);
                        break;
                    }
                case RangeEvaluation.RightToLeft:
                    {
                        value = Mathf.Lerp(_value.y, _value.x, interpolator);
                        break;
                    }
                case RangeEvaluation.Leftmost:
                    {
                        value = _value.x;
                        break;
                    }
                case RangeEvaluation.Rightmost:
                    {
                        value = _value.y;
                        break;
                    }
                case RangeEvaluation.Average:
                    {
                        value = (_value.x + _value.y) / 2f;
                        break;
                    }
                default:
                    {
                        goto case RangeEvaluation.LeftToRight;
                    }
            }

            return _round == true ? Mathf.Round(value) : value;
        }

        #endregion

        #region range interpolation curve

        [SerializeField]
        private CurveConfig _interpolation;

        private float Interpolate(float t)
        {
            if (t < 0f || t > 1f)
            {
                throw new ArgumentOutOfRangeException();
            }

            return _interpolation.Evaluate(1f - t);
        }

        #endregion

        #region debug

        [Button("Log values")]
        private void LogValues()
        {
            IEnumerable<PowerClass> powerClasses = Enum.GetValues(typeof(PowerClass)).Cast<PowerClass>();

            foreach (PowerClass @class in powerClasses)
            {
                float value = Evaluate(@class);
                Debug.Log($"{@class}-class, {value:n2}");
            }
        }

        #endregion
    }
}