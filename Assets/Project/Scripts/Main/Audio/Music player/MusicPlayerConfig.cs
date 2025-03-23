using SpaceAce.Auxiliary.Configs;

using UnityEngine;

namespace SpaceAce.Main.Audio
{
    [CreateAssetMenu(fileName = "Music player config",
                     menuName = "Space Ace/Audio/Music player config")]
    public sealed class MusicPlayerConfig : ScriptableObject
    {
        #region main menu audio

        [SerializeField]
        private AudioCollection _mainMenuAudio;

        [SerializeField]
        private FloatValueConfig _mainMenuPlaybackDelay;

        public AudioCollection MainMenuAudio => _mainMenuAudio;
        public FloatValueConfig MainMenuPlaybackDelay => _mainMenuPlaybackDelay;

        #endregion

        #region battle audio

        [SerializeField, Space]
        private AudioCollection _battleAudio;

        [SerializeField]
        private FloatValueConfig _battlePlaybackDelay;
        
        public AudioCollection BattleAudio => _battleAudio;
        public FloatValueConfig BattlePlaybackDelay => _battlePlaybackDelay;

        #endregion
    }
}