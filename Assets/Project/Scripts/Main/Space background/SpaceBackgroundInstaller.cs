using UnityEngine;

using Zenject;

namespace SpaceAce.Main.SpaceBackgrounds
{
    public sealed class SpaceBackgroundInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _spaceBackgroundPrefab;

        [SerializeField]
        private SpaceBackgroundConfig _config;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SpaceBackground>()
                     .AsSingle()
                     .WithArguments(_spaceBackgroundPrefab, _config)
                     .NonLazy();
        }
    }
}