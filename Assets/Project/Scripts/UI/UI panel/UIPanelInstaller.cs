using UnityEngine;
using UnityEngine.UIElements;

using Zenject;

namespace SpaceAce.UI
{
    public abstract class UIPanelInstaller<T1, T2> : MonoInstaller where T1 : UIPanel
                                                                   where T2 : UIPanelMediator
    {
        [SerializeField]
        private VisualTreeAsset _panel;

        [SerializeField]
        private PanelSettings _settings;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<T1>()
                     .AsSingle()
                     .WithArguments(_panel, _settings)
                     .NonLazy();

            Container.BindInterfacesAndSelfTo<T2>()
                     .AsSingle()
                     .NonLazy();
        }
    }
}