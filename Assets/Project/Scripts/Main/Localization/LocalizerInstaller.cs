using SpaceAce.Main.DI;

using VContainer;

namespace SpaceAce.Main.Localization
{
    public sealed class LocalizerInstaller : ServiceInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            builder.Register<Localizer>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}