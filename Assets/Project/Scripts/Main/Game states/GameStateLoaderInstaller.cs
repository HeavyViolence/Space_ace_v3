using SpaceAce.Main.DI;

using VContainer;

namespace SpaceAce.Main.GameStates
{
    public sealed class GameStateLoaderInstaller : ServiceInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            builder.Register<GameStateLoader>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}