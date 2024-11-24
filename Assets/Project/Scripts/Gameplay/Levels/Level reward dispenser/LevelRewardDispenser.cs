using System;

using Zenject;

namespace SpaceAce.Gameplay.Levels
{
    public sealed class LevelRewardDispenser : IInitializable, IDisposable
    {
        public event EventHandler<RewardEventArgs> RewardDispensed;

        private readonly LevelRewardCollector _levelRewardCollector;

        public LevelRewardDispenser(LevelRewardCollector collector)
        {
            _levelRewardCollector = collector ?? throw new ArgumentNullException();
        }

        #region interfaces

        public void Initialize()
        {
            _levelRewardCollector.RewardCollected += LevelRewardCollectedEventHandler;
        }

        public void Dispose()
        {
            _levelRewardCollector.RewardCollected -= LevelRewardCollectedEventHandler;
        }

        #endregion

        private void LevelRewardCollectedEventHandler(object sender, RewardEventArgs e)
        {
            RewardDispensed?.Invoke(this, e);
        }
    }
}