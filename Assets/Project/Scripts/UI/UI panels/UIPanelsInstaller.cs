using Zenject;

namespace SpaceAce.UI
{
    public sealed class UIPanelsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<UIPanels>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}