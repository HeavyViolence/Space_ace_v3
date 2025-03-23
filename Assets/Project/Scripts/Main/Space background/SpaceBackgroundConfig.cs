using SpaceAce.Auxiliary;
using SpaceAce.Auxiliary.Configs;

using System.Collections.Generic;

using UnityEngine;

namespace SpaceAce.Main.SpaceBackgrounds
{
    [CreateAssetMenu(fileName = "Space background config",
                     menuName = "Space Ace/Main/Space background config")]
    public sealed class SpaceBackgroundConfig : ScriptableObject
    {
        [SerializeField]
        private FloatValueConfig _mainMenuScrollSpeed;

        public FloatValueConfig MainMenuScrollSpeed =>
            _mainMenuScrollSpeed;

        [SerializeField]
        private FloatValueConfig _levelScrollSpeed;

        public FloatValueConfig LevelScrollSpeed =>
            _levelScrollSpeed;

        [SerializeField]
        private List<Material> _spaceBackgrounds;

        public Material GetRandomBackground() =>
            MyMath.GetRandom(_spaceBackgrounds);
    }
}