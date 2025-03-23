using SpaceAce.Main.DI;

using VContainer;

namespace SpaceAce.Main.Controls
{
    public sealed class GameControlsInstaller : ServiceInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            builder.Register<GameControls>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}