using Zenject;

namespace SpaceAce.Main.GamePause
{
    public sealed class GamePauserInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GamePauser>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}