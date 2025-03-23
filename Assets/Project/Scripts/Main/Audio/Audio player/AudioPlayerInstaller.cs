using SpaceAce.Main.DI;

using UnityEngine;
using UnityEngine.Audio;

using VContainer;

namespace SpaceAce.Main.Audio
{
    public sealed class AudioPlayerInstaller : ServiceInstaller
    {
        [SerializeField, Range(AudioPlayer.MinAudioSources, AudioPlayer.MaxAudioSources)]
        private int _audioSources = AudioPlayer.MinAudioSources;

        [SerializeField]
        private AudioMixer _mixer;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<AudioPlayer>(Lifetime.Singleton)
                   .WithParameter(_audioSources)
                   .WithParameter(_mixer)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}