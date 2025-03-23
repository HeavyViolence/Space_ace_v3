using SpaceAce.Main.DI;

using VContainer;

namespace SpaceAce.Main.GamePause
{
    public sealed class GamePauserInstaller : ServiceInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            builder.Register<GamePauser>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}