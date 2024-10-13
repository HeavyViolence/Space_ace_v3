using SpaceAce.Auxiliary;
using SpaceAce.Auxiliary.Configs;

using System.Collections.Generic;

using UnityEngine;

namespace SpaceAce.Main.SpaceBackgrounds
{
    [CreateAssetMenu(fileName = "Space background config",
                     menuName = "Space ace/Configs/Main/Space background config")]
    public sealed class SpaceBackgroundConfig : ScriptableObject
    {
        #region main menu backgrounds

        [SerializeField]
        private FloatValueConfig _mainMenuBackgroundScrollSpeed;

        public FloatValueConfig MainMenuBackgroundScrollSpeed =>
            _mainMenuBackgroundScrollSpeed;

        [SerializeField]
        private List<Material> _mainMenuBackgrounds;

        public Material GetRandomMainMenuBackground() =>
            MyMath.GetRandom(_mainMenuBackgrounds);

        #endregion

        #region space backgrounds

        [SerializeField, Space]
        private FloatValueConfig _spaceBackgroundScrollSpeed;

        public FloatValueConfig SpaceBackgroundScrollSpeed =>
            _spaceBackgroundScrollSpeed;

        [SerializeField]
        private List<Material> _spaceBackgrounds;

        public Material GetRandomSpaceBackground() =>
            MyMath.GetRandom(_spaceBackgrounds);

        #endregion
    }
}