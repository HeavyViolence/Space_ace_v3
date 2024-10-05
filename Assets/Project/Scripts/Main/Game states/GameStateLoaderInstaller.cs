using Zenject;

namespace SpaceAce.Main.GameStates
{
    public sealed class GameStateLoaderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameStateLoader>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}