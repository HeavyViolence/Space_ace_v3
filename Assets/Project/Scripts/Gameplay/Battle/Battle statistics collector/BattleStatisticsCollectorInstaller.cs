using SpaceAce.Main.DI;

using VContainer;

namespace SpaceAce.Gameplay.Battle
{
    public sealed class BattleStatisticsCollectorInstaller : ServiceInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            builder.Register<BattleStatisticsCollector>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}