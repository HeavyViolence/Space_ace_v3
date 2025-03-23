using SpaceAce.Main.DI;

using VContainer;

namespace SpaceAce.Gameplay.Battle
{
    public sealed class BattleDirectorInstaller : ServiceInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            builder.Register<BattleDirector>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}