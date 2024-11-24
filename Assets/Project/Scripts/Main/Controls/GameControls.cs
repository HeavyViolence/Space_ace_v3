using SpaceAce.Auxiliary.EventArguments;
using SpaceAce.Main.GamePause;
using SpaceAce.Main.GameStates;
using SpaceAce.Main.Saving;

using System;

using UnityEngine.InputSystem;

using Zenject;

using static SpaceAce.Main.Controls.ControlsTable;

namespace SpaceAce.Main.Controls
{
    public sealed class GameControls : IInitializable, IDisposable, ISavable
    {
        public event EventHandler SavingRequested;
        public event EventHandler<ErrorOccurredEventArgs> ErrorOccurred;

        private readonly ControlsTable _controls = new();
        private readonly SavingSystem _savingSystem;
        private readonly GamePauser _gamePauser;
        private readonly GameStateLoader _gameStateLoader;

        public NavigationActions Navigation => _controls.Navigation;
        public GameplayActions Gameplay => _controls.Gameplay;

        public string SavedDataName => "Controls";

        public GameControls(SavingSystem savingSystem,
                            GamePauser gamePauser,
                            GameStateLoader gameStateLoader)
        {
            _savingSystem = savingSystem ?? throw new ArgumentNullException();
            _gamePauser = gamePauser ?? throw new ArgumentNullException();
            _gameStateLoader = gameStateLoader ?? throw new ArgumentNullException();
        }

        #region interfaces

        public void Initialize()
        {
            _savingSystem.Register(this);

            Gameplay.Disable();

            _gamePauser.GamePaused += GamePausedEventHandler;
            _gamePauser.GameResumed += GameResumedEventHandler;

            _gameStateLoader.LevelLoaded += LevelLoadedEventHandler;
            _gameStateLoader.MainMenuLoaded += MainMenuLoadedEventHandler;

            InputSystem.onActionChange += ActionChangedEventHandler;
        }

        public void Dispose()
        {
            _savingSystem.Deregister(this);

            _gamePauser.GamePaused -= GamePausedEventHandler;
            _gamePauser.GameResumed -= GameResumedEventHandler;

            _gameStateLoader.LevelLoaded -= LevelLoadedEventHandler;
            _gameStateLoader.MainMenuLoaded -= MainMenuLoadedEventHandler;

            InputSystem.onActionChange -= ActionChangedEventHandler;

            _controls.Disable();
            _controls.Dispose();
        }

        public string GetState()
        {
            try
            {
                string overrides = _controls.SaveBindingOverridesAsJson();
                return overrides;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new(ex));
                return string.Empty;
            }
        }

        public void SetState(string state)
        {
            try
            {
                _controls.Disable();

                if (string.IsNullOrEmpty(state) == false &&
                    string.IsNullOrWhiteSpace(state) == false)
                {
                    _controls.LoadBindingOverridesFromJson(state);
                }
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new(ex));
            }
            finally
            {
                _controls.Enable();
            }
        }

        public override bool Equals(object obj) =>
            obj is not null && Equals(obj as ISavable);

        public bool Equals(ISavable other) =>
            other is not null && SavedDataName == other.SavedDataName;

        public override int GetHashCode() =>
            SavedDataName.GetHashCode();

        #endregion

        #region event handlers

        private void ActionChangedEventHandler(object obj, InputActionChange change)
        {
            if (change == InputActionChange.BoundControlsChanged)
            {
                SavingRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void GamePausedEventHandler(object sender, EventArgs e)
        {
            Gameplay.Disable();
        }

        private void GameResumedEventHandler(object sender, EventArgs e)
        {
            Gameplay.Enable();
        }

        private void LevelLoadedEventHandler(object sender, LevelLoadedEventArgs e)
        {
            Gameplay.Enable();
        }

        private void MainMenuLoadedEventHandler(object sender, EventArgs e)
        {
            Gameplay.Disable();
        }

        #endregion
    }
}