using UnityEngine;

using Zenject;

namespace SpaceAce.Main.MasterCamera
{
    public sealed class MasterCameraShakerInstaller : MonoInstaller
    {
        [SerializeField]
        private MasterCameraShakerConfig _config;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MasterCameraShaker>()
                     .AsSingle()
                     .WithArguments(_config)
                     .NonLazy();
        }
    }
}