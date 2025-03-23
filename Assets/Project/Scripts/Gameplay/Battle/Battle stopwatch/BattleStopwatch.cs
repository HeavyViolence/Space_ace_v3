using SpaceAce.Main.GamePause;
using SpaceAce.Main.GameStates;

using System;
using System.Diagnostics;

using VContainer;
using VContainer.Unity;

namespace SpaceAce.Gameplay.Battle
{
    public sealed class BattleStopwatch : IInitializable, IDisposable
    {
        private readonly GameStateLoader _gameStateLoader;
        private readonly BattleDirector _battleDirector;
        private readonly GamePauser _gamePauser;
        private readonly Stopwatch _stopwatch = new();

        public TimeSpan Time => _stopwatch.Elapsed;
        public bool IsRunning => _stopwatch.IsRunning;

        [Inject]
        public BattleStopwatch(GameStateLoader gameStateLoader,
                               BattleDirector battleDirector,
                               GamePauser gamePauser)
        {
            _gameStateLoader = gameStateLoader ?? throw new ArgumentNullException();
            _battleDirector = battleDirector ?? throw new ArgumentNullException();
            _gamePauser = gamePauser ?? throw new ArgumentNullException();
        }

        #region interfaces

        public void Initialize()
        {
            _gameStateLoader.BattleStateLoaded += OnBattleStateLoaded;

            _battleDirector.BattleStarted += OnBattleStarted;
            _battleDirector.WaveStarted += OnWaveStarted;

            _battleDirector.WaveEnded += OnWaveEnded;
            _battleDirector.BattleEnded += OnBattleEnded;

            _gamePauser.GamePaused += OnGamePaused;
            _gamePauser.GameResumed += OnGameResumed;
        }

        public void Dispose()
        {
            _gameStateLoader.BattleStateLoaded -= OnBattleStateLoaded;

            _battleDirector.BattleStarted -= OnBattleStarted;
            _battleDirector.WaveStarted -= OnWaveStarted;

            _battleDirector.WaveEnded -= OnWaveEnded;
            _battleDirector.BattleEnded -= OnBattleEnded;

            _gamePauser.GamePaused -= OnGamePaused;
            _gamePauser.GameResumed -= OnGameResumed;
        }

        #endregion

        #region vent handlers

        private void OnBattleStateLoaded(BattleDifficulty difficulty)
        {
            _stopwatch.Reset();
        }

        private void OnBattleStarted(BattleDifficulty difficulty)
        {
            _stopwatch.Start();
        }

        private void OnWaveStarted(BattleDifficulty difficulty)
        {
            _stopwatch.Start();
        }

        private void OnWaveEnded(BattleDifficulty difficulty)
        {
            _stopwatch.Stop();
        }

        private void OnBattleEnded(BattleDifficulty difficulty)
        {
            _stopwatch.Stop();
        }

        private void OnGamePaused()
        {
            _stopwatch.Stop();
        }

        private void OnGameResumed()
        {
            _stopwatch.Start();
        }

        #endregion
    }
}