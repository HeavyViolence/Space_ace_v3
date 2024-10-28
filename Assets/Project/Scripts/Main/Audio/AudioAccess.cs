using System;

namespace SpaceAce.Main.Audio
{
    public sealed record AudioAccess
    {
        public static AudioAccess None => new(Guid.Empty, 0f);

        public Guid ID { get; }
        public float PlaybackDuration { get; }

        public AudioAccess(Guid id, float playbackDuration)
        {
            ID = id;
            PlaybackDuration = playbackDuration <= 0f ? throw new ArgumentOutOfRangeException()
                                                      : playbackDuration;
        }
    }
}