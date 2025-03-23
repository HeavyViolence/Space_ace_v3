using SpaceAce.Main.DI;

using UnityEngine;

using VContainer;

namespace SpaceAce.Auxiliary.PrimeNumbers
{
    public sealed class PrimalityEvaluatorInstaller : ServiceInstaller
    {
        [SerializeField, Range(PrimalityEvaluator.MinIterations, PrimalityEvaluator.MaxIterations)]
        private int _iterations = PrimalityEvaluator.MinIterations;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<PrimalityEvaluator>(Lifetime.Singleton)
                   .WithParameter(_iterations)
                   .AsImplementedInterfaces()
                   .AsSelf();
        }
    }
}