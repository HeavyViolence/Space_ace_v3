using Zenject;

namespace SpaceAce.Main.Controls
{
    public sealed class GameControlsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<GameControls>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}