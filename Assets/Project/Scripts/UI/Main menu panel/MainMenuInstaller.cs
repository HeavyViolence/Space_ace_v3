using UnityEngine;
using UnityEngine.UIElements;

using Zenject;

namespace SpaceAce.UI
{
    public sealed class MainMenuInstaller : MonoInstaller
    {
        [SerializeField]
        private VisualTreeAsset _panel;

        [SerializeField]
        private PanelSettings _settings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainMenu>()
                     .AsSingle()
                     .WithArguments(_panel, _settings)
                     .NonLazy();

            Container.BindInterfacesAndSelfTo<MainMenuMediator>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}