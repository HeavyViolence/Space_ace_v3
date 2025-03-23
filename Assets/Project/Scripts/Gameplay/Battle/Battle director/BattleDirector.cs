using SpaceAce.Main.GameStates;

using System;

using VContainer;
using VContainer.Unity;

namespace SpaceAce.Gameplay.Battle
{
    public sealed class BattleDirector : IInitializable, IDisposable
    {
        public event Action<BattleDifficulty> BattleStarted, BattleEnded;
        public event Action<BattleDifficulty> WaveStarted, WaveEnded, WaveElevated;

        private readonly GameStateLoader _gameStateLoader;

        [Inject]
        public BattleDirector(GameStateLoader gameStateLoader)
        {
            _gameStateLoader = gameStateLoader ?? throw new ArgumentNullException(nameof(gameStateLoader));
        }

        #region interfaces

        public void Initialize()
        {
            _gameStateLoader.BattleStateLoaded += OnBattleLoaded;
        }

        public void Dispose()
        {
            _gameStateLoader.BattleStateLoaded -= OnBattleLoaded;
        }

        #endregion

        #region event handlers

        private void OnBattleLoaded(BattleDifficulty difficulty)
        {

        }

        #endregion
    }
}