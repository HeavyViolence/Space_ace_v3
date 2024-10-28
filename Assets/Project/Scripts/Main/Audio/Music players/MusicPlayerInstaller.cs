using UnityEngine;

using Zenject;

namespace SpaceAce.Main.Audio
{
    public sealed class MusicPlayerInstaller : MonoInstaller
    {
        [SerializeField]
        private AudioCollection _music;

        [SerializeField]
        private MusicPlayerType _musicPlayerType;

        public override void InstallBindings()
        {
            switch (_musicPlayerType)
            {
                case MusicPlayerType.Continuous:
                    {
                        Container.BindInterfacesAndSelfTo<ContinuousMusicPlayer>()
                                 .AsSingle()
                                 .WithArguments(_music)
                                 .NonLazy();

                        break;
                    }
                case MusicPlayerType.LevelsOnly:
                    {
                        Container.BindInterfacesAndSelfTo<LevelsOnlyMusicPlayer>()
                                 .AsSingle()
                                 .WithArguments(_music)
                                 .NonLazy();

                        break;
                    }
                default:
                    {
                        goto case MusicPlayerType.Continuous;
                    }
            }
        }
    }
}