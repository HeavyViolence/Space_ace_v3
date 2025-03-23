using SpaceAce.Main.DI;

using UnityEngine;

using VContainer;

namespace SpaceAce.Gameplay.Items
{
    public sealed class ItemEvaluatorInstaller : ServiceInstaller
    {
        [SerializeField]
        private ItemEvaluatorConfig _config;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<ItemEvaluator>(Lifetime.Singleton)
                   .WithParameter(_config)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}