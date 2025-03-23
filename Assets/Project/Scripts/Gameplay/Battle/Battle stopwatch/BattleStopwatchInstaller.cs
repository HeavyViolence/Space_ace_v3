using SpaceAce.Main.DI;

using VContainer;

namespace SpaceAce.Gameplay.Battle
{
    public sealed class BattleStopwatchInstaller : ServiceInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            builder.Register<BattleStopwatch>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}