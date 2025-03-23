using SpaceAce.Main.DI;

using UnityEngine;

using VContainer;

namespace SpaceAce.Main.Audio
{
    public sealed class MusicPlayerInstaller : ServiceInstaller
    {
        [SerializeField]
        private MusicPlayerConfig _config;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<MusicPlayer>(Lifetime.Singleton)
                   .WithParameter(_config)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}