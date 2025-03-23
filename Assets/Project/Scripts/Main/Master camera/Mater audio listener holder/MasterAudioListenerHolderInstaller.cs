using SpaceAce.Main.DI;

using VContainer;

namespace SpaceAce.Main.MasterCamera
{
    public sealed class MasterAudioListenerHolderInstaller : ServiceInstaller
    {
        public override void Install(IContainerBuilder builder)
        {
            builder.Register<MasterAudioListenerHolder>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}