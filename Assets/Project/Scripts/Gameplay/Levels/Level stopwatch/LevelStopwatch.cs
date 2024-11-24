using SpaceAce.Main.GamePause;
using SpaceAce.Main.GameStates;

using System;
using System.Diagnostics;

using Zenject;

namespace SpaceAce.Gameplay.Levels
{
    public sealed class LevelStopwatch : IInitializable, IDisposable
    {
        private readonly GameStateLoader _gameStateLoader;
        private readonly LevelCompleter _levelCompleter;
        private readonly GamePauser _gamePauser;
        private readonly Stopwatch _stopwatch = new();

        public TimeSpan Time => _stopwatch.Elapsed;
        public bool IsRunning => _stopwatch.IsRunning;

        public LevelStopwatch(GameStateLoader gameStateLoader,
                              LevelCompleter levelCompleter,
                              GamePauser gamePauser)
        {
            _gameStateLoader = gameStateLoader ?? throw new ArgumentNullException();
            _levelCompleter = levelCompleter ?? throw new ArgumentNullException();
            _gamePauser = gamePauser ?? throw new ArgumentNullException();
        }

        #region interfaces

        public void Initialize()
        {
            _gameStateLoader.LevelLoaded += (_, _) => _stopwatch.Start();
            _gameStateLoader.MainMenuLoaded += (_, _) => _stopwatch.Reset();

            _levelCompleter.LevelConcluded += (_, _) => _stopwatch.Stop();

            _gamePauser.GamePaused += (_, _) => _stopwatch.Stop();
            _gamePauser.GameResumed += (_, _) => _stopwatch.Start();
        }

        public void Dispose()
        {
            _gameStateLoader.LevelLoaded -= (_, _) => _stopwatch.Start();
            _gameStateLoader.MainMenuLoaded -= (_, _) => _stopwatch.Reset();

            _levelCompleter.LevelConcluded -= (_, _) => _stopwatch.Stop();

            _gamePauser.GamePaused -= (_, _) => _stopwatch.Stop();
            _gamePauser.GameResumed -= (_, _) => _stopwatch.Start();
        }

        #endregion
    }
}