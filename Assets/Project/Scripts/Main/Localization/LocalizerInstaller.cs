using UnityEngine;

using Zenject;

namespace SpaceAce.Main.Localization
{
    public sealed class LocalizerInstaller : MonoInstaller
    {
        [SerializeField]
        private LocalizerConfig _config;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Localizer>()
                     .AsSingle()
                     .WithArguments(_config)
                     .NonLazy();
        }
    }
}