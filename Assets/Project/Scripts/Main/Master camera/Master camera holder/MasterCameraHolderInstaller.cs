using SpaceAce.Main.DI;

using UnityEngine;

using VContainer;

namespace SpaceAce.Main.MasterCamera
{
    public sealed class MasterCameraHolderInstaller : ServiceInstaller
    {
        [SerializeField]
        private GameObject _masterCameraPrefab;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<MasterCameraHolder>(Lifetime.Singleton)
                   .WithParameter(_masterCameraPrefab)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}