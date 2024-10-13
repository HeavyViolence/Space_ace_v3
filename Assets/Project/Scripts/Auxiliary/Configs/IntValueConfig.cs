using NaughtyAttributes;

using UnityEngine;

namespace SpaceAce.Auxiliary.Configs
{
    [CreateAssetMenu(fileName = "Int value config",
                     menuName = "Space ace/Configs/Value configs/Int value config")]
    public sealed class IntValueConfig : ScriptableObject
    {
        private const int MinValue = 0;
        private const int MaxValue = 10_000;

        [SerializeField, MinMaxSlider(MinValue, MaxValue)]
        private Vector2Int _valueRange = new(MinValue, MaxValue);

        public int Min => _valueRange.x;
        public int Max => _valueRange.y;
        public int Delta => _valueRange.y - _valueRange.x;
        public int Random => UnityEngine.Random.Range(_valueRange.x, _valueRange.y + 1);
        public int Average => (_valueRange.x + _valueRange.y) / 2;
        public bool IsRanged => _valueRange.x != _valueRange.y;
        public Vector2Int Range => _valueRange;
    }
}