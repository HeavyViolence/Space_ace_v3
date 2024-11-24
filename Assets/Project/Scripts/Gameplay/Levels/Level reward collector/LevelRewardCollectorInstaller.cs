using UnityEngine;

using Zenject;

namespace SpaceAce.Gameplay.Levels
{
    public sealed class LevelRewardCollectorInstaller : MonoInstaller
    {
        [SerializeField]
        private LevelRewardCollectorConfig _config;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelRewardCollector>()
                     .AsSingle()
                     .WithArguments(_config)
                     .NonLazy();
        }
    }
}