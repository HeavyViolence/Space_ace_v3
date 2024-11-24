using UnityEngine;

namespace SpaceAce.Main.Audio
{
    [CreateAssetMenu(fileName = "Background audio",
                     menuName = "Space ace/Configs/Audio/Background audio")]
    public sealed class BackgroundAudio : ScriptableObject
    {
        [SerializeField]
        private AudioCollection _mainMenuAudio;

        [SerializeField]
        private AudioCollection _levelAudio;

        public AudioCollection MainMenuAudio => _mainMenuAudio;
        public AudioCollection LevelAudio => _levelAudio;
    }
}