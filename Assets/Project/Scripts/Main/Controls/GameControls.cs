using SpaceAce.Main.GamePause;
using SpaceAce.Main.GameStates;

using System;

using VContainer;
using VContainer.Unity;

using static SpaceAce.Main.Controls.ControlsTable;

namespace SpaceAce.Main.Controls
{
    public sealed class GameControls : IInitializable, IDisposable
    {
        private readonly ControlsTable _controls = new();
        private readonly GamePauser _gamePauser;
        private readonly GameStateLoader _gameStateLoader;

        public GameplayActions Gameplay => _controls.Gameplay;

        public string StateName => "Controls";

        [Inject]
        public GameControls(GamePauser gamePauser,
                            GameStateLoader gameStateLoader)
        {
            _gamePauser = gamePauser ?? throw new ArgumentNullException();
            _gameStateLoader = gameStateLoader ?? throw new ArgumentNullException();
        }

        #region interfaces

        public void Initialize()
        {
            Gameplay.Disable();

            _gamePauser.GamePaused += OnGamePaused;
            _gamePauser.GameResumed += OnGameResumed;

            _gameStateLoader.BattleStateLoaded += OnBattleStateLoaded;
            _gameStateLoader.MainMenuLoaded += OnMainMenuLoaded;
        }

        public void Dispose()
        {
            _gamePauser.GamePaused -= OnGamePaused;
            _gamePauser.GameResumed -= OnGameResumed;

            _gameStateLoader.BattleStateLoaded -= OnBattleStateLoaded;
            _gameStateLoader.MainMenuLoaded -= OnMainMenuLoaded;

            _controls.Disable();
            _controls.Dispose();
        }

        #endregion

        #region event handlers

        private void OnGamePaused()
        {
            Gameplay.Disable();
        }

        private void OnGameResumed()
        {
            Gameplay.Enable();
        }

        private void OnBattleStateLoaded(BattleDifficulty difficulty)
        {
            Gameplay.Enable();
        }

        private void OnMainMenuLoaded()
        {
            Gameplay.Disable();
        }

        #endregion
    }
}