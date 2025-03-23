using SpaceAce.Main.DI;

using UnityEngine;

using VContainer;

namespace SpaceAce.Auxiliary.Easing
{
    public sealed class EasingServiceInstaller : ServiceInstaller
    {
        [SerializeField]
        private EasingServiceConfig _config;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<EasingService>(Lifetime.Singleton)
                   .WithParameter(_config)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}