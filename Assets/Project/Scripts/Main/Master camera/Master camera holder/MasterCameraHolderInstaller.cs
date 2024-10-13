using UnityEngine;

using Zenject;

namespace SpaceAce.Main.MasterCamera
{
    public sealed class MasterCameraHolderInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _masterCameraPrefab;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MasterCameraHolder>()
                     .AsSingle()
                     .WithArguments(_masterCameraPrefab)
                     .NonLazy();
        }
    }
}