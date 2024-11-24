using Zenject;

namespace SpaceAce.Gameplay.Levels
{
    public sealed class LevelRewardDispenserInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelRewardDispenser>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}