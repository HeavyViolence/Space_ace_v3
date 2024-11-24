using Zenject;

namespace SpaceAce.Gameplay.Levels
{
    public sealed class LevelCompleterInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelCompleter>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}