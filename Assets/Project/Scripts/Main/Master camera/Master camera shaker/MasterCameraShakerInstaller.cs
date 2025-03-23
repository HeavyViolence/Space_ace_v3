using SpaceAce.Main.DI;

using UnityEngine;

using VContainer;

namespace SpaceAce.Main.MasterCamera
{
    public sealed class MasterCameraShakerInstaller : ServiceInstaller
    {
        [SerializeField]
        private MasterCameraShakerConfig _config;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<MasterCameraShaker>(Lifetime.Singleton)
                   .WithParameter(_config)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}