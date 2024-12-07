using SpaceAce.Auxiliary.Easing;

using System;

using UnityEngine;
using UnityEngine.Audio;

namespace SpaceAce.Main.Audio
{
    public sealed record AudioProperties
    {
        public AudioClip Clip { get; }
        public AudioMixerGroup OutputAudioGroup { get; }
        public float Volume { get; }
        public AudioPriority Priority { get; }
        public float SpatialBlend { get; }
        public float Pitch { get; }
        public bool PlayWithEasing { get; }
        public bool CancelWithEasing { get; }
        public float CancellationDuration { get; }
        public EasingMode CancellationEasing { get; }

        public AudioProperties(AudioClip clip,
                               AudioMixerGroup group,
                               float volume,
                               AudioPriority priority,
                               float spatialBlend,
                               float pitch,
                               bool playWithEasing,
                               bool cancelWithEasing,
                               float cancellationDuration,
                               EasingMode cancellationEasing)
        {
            Clip = clip == null ? throw new ArgumentNullException() : clip;
            OutputAudioGroup = group == null ? throw new ArgumentNullException() : group;
            Volume = Mathf.Clamp01(volume);
            Priority = priority;
            SpatialBlend = Mathf.Clamp01(spatialBlend);
            Pitch = Mathf.Clamp(pitch, AudioCollection.MinPitch, AudioCollection.MaxPitch);
            PlayWithEasing = playWithEasing;
            CancelWithEasing = cancelWithEasing;
            CancellationDuration = Mathf.Clamp(cancellationDuration, AudioCollection.MinCancellationDuration, AudioCollection.MaxCancellationDuration);
            CancellationEasing = cancellationEasing;
        }
    }
}