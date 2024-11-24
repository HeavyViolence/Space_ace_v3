using UnityEngine;

namespace SpaceAce.Main.Audio
{
    [CreateAssetMenu(fileName = "UI audio",
                     menuName = "Space ace/Configs/Audio/UI audio")]
    public sealed class UIAudio : ScriptableObject
    {
        [SerializeField]
        private AudioCollection _click;

        public AudioCollection Click => _click;

        [SerializeField]
        private AudioCollection _specialClick;

        public AudioCollection SpecialClick => _specialClick;

        [SerializeField]
        private AudioCollection _clickBack;

        public AudioCollection ClickBack => _clickBack;

        [SerializeField]
        private AudioCollection _switch;

        public AudioCollection Switch => _switch;

        [SerializeField]
        private AudioCollection _hoverOver;

        public AudioCollection HoverOver => _hoverOver;

        [SerializeField, Space]
        private AudioCollection _error;

        public AudioCollection Error => _error;

        [SerializeField, Space]
        private AudioCollection _money;

        public AudioCollection Money => _money;

        [SerializeField]
        private AudioCollection _unlocked;

        public AudioCollection Unlocked => _unlocked;

        [SerializeField]
        private AudioCollection _notification;

        public AudioCollection Notification => _notification;

        [SerializeField, Space]
        private AudioCollection _win;

        public AudioCollection Win => _win;

        [SerializeField]
        private AudioCollection _lost;

        public AudioCollection Lost => _lost;
    }
}