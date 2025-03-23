using SpaceAce.Main.Audio;
using SpaceAce.Main.DI;

using UnityEngine;

using VContainer;

namespace SpaceAce.Main
{
    public sealed class MainServicesInstaller : ServiceInstaller
    {
        [SerializeField]
        private GUIAudio _guiAudio;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<MainServices>(Lifetime.Singleton)
                   .WithParameter(_guiAudio)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}