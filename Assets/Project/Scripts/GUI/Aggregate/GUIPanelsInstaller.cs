using SpaceAce.Main.DI;

using VContainer;

namespace SpaceAce.GUI
{
    public sealed class GUIPanelsInstaller : ServiceInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            builder.Register<GUIPanels>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}