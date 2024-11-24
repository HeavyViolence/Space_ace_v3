using UnityEngine;

namespace SpaceAce.Main.Audio
{
    public sealed record BackgroundAudioPlayerSettings
    {
        public const float MinPlaybackDelay = 0f;
        public const float MaxPlaybackDelay = 300f;
        public const float DefaultPlaybackDelay = 60f;

        public static readonly BackgroundAudioPlayerSettings Default =
            new(true, DefaultPlaybackDelay, true, DefaultPlaybackDelay, DefaultPlaybackDelay);

        public bool MainMenuBackgroundAudioEnabled { get; }
        public float MainMenuPlaybackDelay { get; }

        public bool LevelBackgroundAudioEnabled { get; }
        public float LevelFirstPlaybackDelay { get; }
        public float LevelPlaybackDelay { get; }

        public BackgroundAudioPlayerSettings(bool mainMenuBackgroundAudioEnabled,
                                             float mainMenuPlaybackDelay,
                                             bool levelBackgroundAudioEnabled,
                                             float levelFirstPlaybackDelay,
                                             float levelPlaybackDelay)
        {
            MainMenuBackgroundAudioEnabled = mainMenuBackgroundAudioEnabled;
            MainMenuPlaybackDelay = Mathf.Clamp(mainMenuPlaybackDelay, MinPlaybackDelay, MaxPlaybackDelay);

            LevelBackgroundAudioEnabled = levelBackgroundAudioEnabled;
            LevelFirstPlaybackDelay = Mathf.Clamp(levelFirstPlaybackDelay, MinPlaybackDelay, MaxPlaybackDelay);
            LevelPlaybackDelay = Mathf.Clamp(levelPlaybackDelay, MinPlaybackDelay, MaxPlaybackDelay);
        }
    }
}