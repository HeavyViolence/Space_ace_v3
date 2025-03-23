using System;

using VContainer;

namespace SpaceAce.Gameplay.Items
{
    public sealed class ItemEvaluator
    {
        private readonly ItemEvaluatorConfig _config;

        [Inject]
        public ItemEvaluator(ItemEvaluatorConfig config)
        {
            _config = config == null ? throw new ArgumentNullException() : config;
        }

        public PowerClassData GetPowerClassData(PowerClass @class) => _config.Data[@class];
    }
}