using SpaceAce.Main.DI;

using UnityEngine;

using VContainer;

namespace SpaceAce.GUI
{
    public sealed class MainMenuInstaller : ServiceInstaller
    {
        [SerializeField]
        private MainMenu _mainMenu;

        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterInstance(_mainMenu)
                   .AsImplementedInterfaces()
                   .AsSelf();

            builder.Register<MainMenuMediator>(Lifetime.Singleton)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}