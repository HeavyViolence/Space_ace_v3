using UnityEngine;

using Zenject;

namespace SpaceAce.Main.Audio
{
    public sealed class BackgroundAudioPlayerInstaller : MonoInstaller
    {
        [SerializeField]
        private BackgroundAudio _audio;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BackgroundAudioPlayer>()
                     .AsSingle()
                     .WithArguments(_audio)
                     .NonLazy();
        }
    }
}