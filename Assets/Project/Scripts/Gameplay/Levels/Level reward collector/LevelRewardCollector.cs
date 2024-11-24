using SpaceAce.Auxiliary.Observables;
using SpaceAce.Main.GameStates;

using System;

using Zenject;

namespace SpaceAce.Gameplay.Levels
{
    public sealed class LevelRewardCollector : IInitializable, IDisposable
    {
        public event EventHandler<RewardEventArgs> RewardCollected;

        private readonly LevelRewardCollectorConfig _config;
        private readonly GameStateLoader _gameStateLaoder;
        private readonly LevelCompleter _levelCompleter;

        public ObservableValue<float> CreditsReward { get; private set; }
        public ObservableValue<float> ExperienceReward { get; private set; }

        public LevelRewardCollector(LevelRewardCollectorConfig config,
                                    GameStateLoader gameStateLoader,
                                    LevelCompleter levelCompleter)
        {
            _config = config == null ? throw new ArgumentNullException() : config;
            _gameStateLaoder = gameStateLoader ?? throw new ArgumentNullException();
            _levelCompleter = levelCompleter ?? throw new ArgumentNullException();
        }

        #region interfaces

        public void Initialize()
        {
            _gameStateLaoder.LevelLoadingStarted += LevelLoadingStartedEventHandler;
            _gameStateLaoder.MainMenuLoaded += MainMenuLoadedEventHandler;

            _levelCompleter.LevelCompleted += LevelCompletedEventHandler;
        }

        public void Dispose()
        {
            _gameStateLaoder.LevelLoadingStarted -= LevelLoadingStartedEventHandler;
            _gameStateLaoder.MainMenuLoaded -= MainMenuLoadedEventHandler;

            _levelCompleter.LevelCompleted -= LevelCompletedEventHandler;
        }

        #endregion

        #region event handlers

        private void LevelLoadingStartedEventHandler(object sender, LevelLoadingStartedEventArgs e)
        {
            CreditsReward.Value = _config.GetCreditsReward(e.Level);
            ExperienceReward.Value = _config.GetExperienceReward(e.Level);
        }

        private void MainMenuLoadedEventHandler(object sender, EventArgs e)
        {
            CreditsReward.Value = 0f;
            ExperienceReward.Value = 0f;
        }

        private void LevelCompletedEventHandler(object sender, LevelEventArgs e)
        {
            RewardCollected?.Invoke(this, new(CreditsReward.Value, ExperienceReward.Value));
        }

        #endregion
    }
}