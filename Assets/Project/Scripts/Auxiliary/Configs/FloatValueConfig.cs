using NaughtyAttributes;

using UnityEngine;

namespace SpaceAce.Auxiliary.Configs
{
    [CreateAssetMenu(fileName = "Float value config",
                     menuName = "Space Ace/Value configs/Float value config")]
    public sealed class FloatValueConfig : ScriptableObject
    {
        private const float MinValue = 0f;
        private const float MaxValue = 10_000f;

        [SerializeField, MinMaxSlider(MinValue, MaxValue)]
        private Vector2 _valueRange = new(MinValue, MaxValue);

        public float Min => _valueRange.x;
        public float Max => _valueRange.y;
        public float Delta => _valueRange.y - _valueRange.x;
        public float Random => UnityEngine.Random.Range(_valueRange.x, _valueRange.y);
        public float Average => (_valueRange.x + _valueRange.y) / 2f;
        public bool IsRanged => _valueRange.x != _valueRange.y;
        public Vector2 Range => _valueRange;
    }
}