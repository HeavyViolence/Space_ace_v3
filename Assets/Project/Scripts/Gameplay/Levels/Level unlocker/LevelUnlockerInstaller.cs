using UnityEngine;

using Zenject;

namespace SpaceAce.Gameplay.Levels
{
    public sealed class LevelUnlockerInstaller : MonoInstaller
    {
        private const int MinLastUnlockedLevel = 1;
        private const int MaxLastUnlockedLevel = 100;

        [SerializeField, Range(MinLastUnlockedLevel, MaxLastUnlockedLevel)]
        private int _lastUnlockedLevel = MinLastUnlockedLevel;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<LevelUnlocker>()
                     .AsSingle()
                     .WithArguments(_lastUnlockedLevel)
                     .NonLazy();
        }
    }
}