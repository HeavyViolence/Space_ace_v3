using Newtonsoft.Json;

using SpaceAce.Auxiliary.EventArguments;
using SpaceAce.Main.GameStates;
using SpaceAce.Main.Saving;

using System;
using System.Collections.Generic;

using Zenject;

namespace SpaceAce.Gameplay.Levels
{
    public sealed class BestLevelRunStatisticsCollector : IInitializable, IDisposable, ISavable
    {
        public event EventHandler SavingRequested;
        public event EventHandler<ErrorOccurredEventArgs> ErrorOccurred;

        private readonly SavingSystem _savingSystem;
        private readonly GameStateLoader _gameStateLoader;
        private readonly LevelCompleter _levelCompleter;
        private readonly LevelStopwatch _levelStopwatch;
        private readonly LevelStatisticsCache _levelStatisticsCache = new();

        private Dictionary<int, LevelStatistics> _statistics = new();

        public string SavedDataName => "Statistics";

        public BestLevelRunStatisticsCollector(SavingSystem savingSystem,
                                               GameStateLoader gameStateLoader,
                                               LevelCompleter levelCompleter,
                                               LevelStopwatch levelStopwatch)
        {
            _savingSystem = savingSystem ?? throw new ArgumentNullException();
            _gameStateLoader = gameStateLoader ?? throw new ArgumentNullException();
            _levelCompleter = levelCompleter ?? throw new ArgumentNullException();
            _levelStopwatch = levelStopwatch ?? throw new ArgumentNullException();
        }

        public bool TryGetStatistics(int level, out LevelStatistics statistics)
        {
            if (level <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (_statistics.TryGetValue(level, out LevelStatistics s) == true)
            {
                statistics = s;
                return true;
            }

            statistics = null;
            return false;
        }

        #region interfaces

        public void Initialize()
        {
            _savingSystem.Register(this);

            _gameStateLoader.LevelLoaded += LevelLoadedEventHandler;
            _levelCompleter.LevelCompleted += LevelCompletedEventHandler;
        }

        public void Dispose()
        {
            _savingSystem.Deregister(this);

            _gameStateLoader.LevelLoaded -= LevelLoadedEventHandler;
            _levelCompleter.LevelCompleted -= LevelCompletedEventHandler;
        }

        public string GetState() =>
            JsonConvert.SerializeObject(_statistics, Formatting.Indented);

        public void SetState(string state)
        {
            try
            {
                _statistics = JsonConvert.DeserializeObject<Dictionary<int, LevelStatistics>>(state);
            }
            catch (Exception ex)
            {
                _statistics = new();
                ErrorOccurred?.Invoke(this, new(ex));
            }
        }

        public override bool Equals(object obj) =>
            obj is not null && Equals(obj as ISavable);

        public bool Equals(ISavable other) =>
            other is not null && SavedDataName == other.SavedDataName;

        public override int GetHashCode() =>
            SavedDataName.GetHashCode();

        #endregion

        #region even handlers

        private void LevelLoadedEventHandler(object sender, LevelLoadedEventArgs e)
        {
            _levelStatisticsCache.Reset();
        }

        private void LevelCompletedEventHandler(object sender, LevelEventArgs e)
        {
            if (_statistics.TryGetValue(e.Level, out LevelStatistics statistics) == true)
            {
                LevelStatistics completedLevelStatistics = _levelStatisticsCache.GetSnapshot(_levelStopwatch.Time);

                if (completedLevelStatistics > statistics)
                {
                    _statistics.Add(e.Level, completedLevelStatistics);
                    SavingRequested?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        #endregion
    }
}