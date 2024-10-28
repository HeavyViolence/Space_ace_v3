using System;

using UnityEngine;

namespace SpaceAce.Main.Audio
{
    public sealed record AudioSourceCache
    {
        public AudioSource AudioSource { get; }
        public Transform Transform { get; }

        public AudioSourceCache(AudioSource source, Transform transform)
        {
            AudioSource = source == null ? throw new ArgumentNullException() : source;
            Transform = transform == null ? throw new ArgumentNullException() : transform;
        }
    }
}