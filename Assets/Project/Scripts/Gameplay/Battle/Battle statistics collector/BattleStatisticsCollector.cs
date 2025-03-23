using MessagePack;

using SpaceAce.Main.GameStates;
using SpaceAce.Main.Saving;

using System;

using VContainer;
using VContainer.Unity;

namespace SpaceAce.Gameplay.Battle
{
    public sealed class BattleStatisticsCollector : IInitializable, IDisposable, ISavable
    {
        public event Action StateChanged;

        private readonly SavingSystem _savingSystem;
        private readonly GameStateLoader _gameStateLoader;
        private readonly BattleDirector _battleDirector;
        private readonly BattleStopwatch _levelStopwatch;

        private BattleStatisticsCache _currentBattleStatisticsCache;

        public BattleStatistics Statistics { get; private set; } = BattleStatistics.Default;
        public string StateName => "Statistics";

        [Inject]
        public BattleStatisticsCollector(SavingSystem savingSystem,
                                         GameStateLoader gameStateLoader,
                                         BattleDirector battleDirector,
                                         BattleStopwatch levelStopwatch)
        {
            _savingSystem = savingSystem ?? throw new ArgumentNullException();
            _gameStateLoader = gameStateLoader ?? throw new ArgumentNullException();
            _battleDirector = battleDirector ?? throw new ArgumentNullException();
            _levelStopwatch = levelStopwatch ?? throw new ArgumentNullException();
        }

        #region interfaces

        public void Initialize()
        {
            _savingSystem.Register(this);

            _gameStateLoader.BattleStateLoaded += OnBattleStateLoaded;
            _battleDirector.BattleEnded += OnBattleEnded;
        }

        public void Dispose()
        {
            _savingSystem.Deregister(this);

            _gameStateLoader.BattleStateLoaded -= OnBattleStateLoaded;
            _battleDirector.BattleEnded -= OnBattleEnded;
        }

        public byte[] GetState() => MessagePackSerializer.Serialize(Statistics);

        public void SetState(byte[] state)
        {
            try
            {
                Statistics = MessagePackSerializer.Deserialize<BattleStatistics>(state);
            }
            catch (Exception)
            {
                Statistics = BattleStatistics.Default;
            }
        }

        #endregion

        #region even handlers

        private void OnBattleStateLoaded(BattleDifficulty difficulty)
        {
            _currentBattleStatisticsCache = new();
        }

        private void OnBattleEnded(BattleDifficulty difficulty)
        {
            BattleStatistics completedBattleStatistics = _currentBattleStatisticsCache.GetSnapshot(_levelStopwatch.Time);

            if (completedBattleStatistics > Statistics)
            {
                Statistics = completedBattleStatistics;
                StateChanged?.Invoke();
            }
        }

        #endregion
    }
}