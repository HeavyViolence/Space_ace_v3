using SpaceAce.Main.DI;

using UnityEngine;

using VContainer;

namespace SpaceAce.Main.SpaceBackgrounds
{
    public sealed class SpaceBackgroundInstaller : ServiceInstaller
    {
        [SerializeField]
        private GameObject _spaceBackgroundPrefab;

        [SerializeField]
        private SpaceBackgroundConfig _config;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<SpaceBackground>(Lifetime.Singleton)
                   .WithParameter(_spaceBackgroundPrefab)
                   .WithParameter(_config)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}