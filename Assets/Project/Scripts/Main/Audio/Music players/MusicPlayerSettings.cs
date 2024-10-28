using UnityEngine;

namespace SpaceAce.Main.Audio
{
    public sealed record MusicPlayerSettings
    {
        public const float MinPlaybackDelay = 0f;
        public const float MaxPlaybackDelay = 300f;
        public const float DefaultPlaybackDelay = 60f;

        public static MusicPlayerSettings Default => new(MinPlaybackDelay, DefaultPlaybackDelay);

        public float FirstPlaybackDelay { get; }
        public float PlaybackDelay { get; }

        public MusicPlayerSettings(float firstPlaybackDelay, float playbackDelay)
        {
            FirstPlaybackDelay = Mathf.Clamp(firstPlaybackDelay, MinPlaybackDelay, MaxPlaybackDelay);
            PlaybackDelay = Mathf.Clamp(playbackDelay, MinPlaybackDelay, MaxPlaybackDelay);
        }
    }
}