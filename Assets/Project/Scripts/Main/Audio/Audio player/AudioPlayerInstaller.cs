using UnityEngine;
using UnityEngine.Audio;

using Zenject;

namespace SpaceAce.Main.Audio
{
    public sealed class AudioPlayerInstaller : MonoInstaller
    {
        [SerializeField, Range(AudioPlayer.MinAudioSources, AudioPlayer.MaxAudioSources)]
        private int _audioSources = AudioPlayer.MinAudioSources;

        [SerializeField]
        private AudioMixer _mixer;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AudioPlayer>()
                     .AsSingle()
                     .WithArguments(_audioSources, _mixer)
                     .NonLazy();
        }
    }
}