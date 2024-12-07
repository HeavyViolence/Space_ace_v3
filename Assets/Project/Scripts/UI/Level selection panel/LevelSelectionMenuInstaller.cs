using UnityEngine;
using UnityEngine.UIElements;

using Zenject;

namespace SpaceAce.UI
{
    public sealed class LevelSelectionMenuInstaller : MonoInstaller
    {
        [SerializeField]
        private VisualTreeAsset _panel;

        [SerializeField]
        private PanelSettings _settings;

        [SerializeField]
        private LevelSelectionMenuConfig _config;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelSelectionMenu>()
                     .AsSingle()
                     .WithArguments(_panel, _settings, _config)
                     .NonLazy();

            Container.BindInterfacesAndSelfTo<LevelSelectionMenuMediator>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}