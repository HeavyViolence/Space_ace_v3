using NaughtyAttributes;

using SpaceAce.Auxiliary.Configs;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace SpaceAce.Gameplay.Items
{
    [CreateAssetMenu(fileName = "Item int property config",
                     menuName = "Space Ace/Gameplay/Items/Related/Item int property config")]
    public sealed class ItemIntPropertyConfig : ScriptableObject
    {
        #region value range

        private static readonly float s_powerClassCount = Enum.GetValues(typeof(PowerClass)).Length - 1;

        public const int MinValue = 0;
        public const int MaxValue = 9_999;

        [SerializeField, MinMaxSlider(MinValue, MaxValue)]
        private Vector2Int _value = new(MinValue, MaxValue);

        [SerializeField]
        private RangeEvaluation _evaluation = RangeEvaluation.LeftToRight;

        public int Evaluate(PowerClass @class)
        {
            float t = (float)@class / s_powerClassCount;
            float interpolator = Interpolate(t);
            int value;

            switch (_evaluation)
            {
                case RangeEvaluation.LeftToRight:
                    {
                        value = Mathf.RoundToInt(_value.x + (_value.y - _value.x) * interpolator);
                        break;
                    }
                case RangeEvaluation.RightToLeft:
                    {
                        value = Mathf.RoundToInt(_value.y + (_value.x - _value.y) * interpolator);
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
                        value = (_value.x + _value.y) / 2;
                        break;
                    }
                default:
                    {
                        goto case RangeEvaluation.LeftToRight;
                    }
            }

            return value;
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