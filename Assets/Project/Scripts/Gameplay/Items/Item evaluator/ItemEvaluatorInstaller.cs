using UnityEngine;

using Zenject;

namespace SpaceAce.Gameplay.Items
{
    public sealed class ItemEvaluatorInstaller : MonoInstaller
    {
        [SerializeField]
        private ItemEvaluatorConfig _config;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<ItemEvaluator>()
                     .AsSingle()
                     .WithArguments(_config)
                     .NonLazy();
        }
    }
}