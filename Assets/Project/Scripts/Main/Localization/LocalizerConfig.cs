using UnityEngine;
using UnityEngine.Localization;

namespace SpaceAce.Main.Localization
{
    [CreateAssetMenu(fileName = "Localizer config",
                     menuName = "Space ace/Configs/Main/Localizer config")]
    public sealed class LocalizerConfig : ScriptableObject
    {
        [SerializeField]
        private LocalizedFont _regularFont;

        public LocalizedFont RegularFont => _regularFont;

        [SerializeField]
        private Language _initialLanguage;

        public Language InitialLanguage => _initialLanguage;
    }
}