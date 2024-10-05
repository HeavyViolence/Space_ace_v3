using Cysharp.Threading.Tasks;

using System;

using UnityEngine;

namespace SpaceAce.Main.GameStates
{
    public sealed class GameStateLoader
    {
        public const float MinLoadDelay = 0f;
        public const float MaxLoadDelay = 10f;

        public event EventHandler<MainMenuLoadingStartedEventArgs> MainMenuLoadingStarted;
        public event EventHandler MainMenuLoaded;

        public event EventHandler<LevelLoadingStartedEventArgs> LevelLoadingStarted;
        public event EventHandler<LevelLoadedEventArgs> LevelLoaded;

        public GameState CurrentState { get; private set; } = GameState.MainMenu;
        public int LoadedLevel { get; private set; } = 0;

        public async UniTask LoadMainMenuAsync(float delay)
        {
            if (CurrentState == GameState.MainMenu ||
                CurrentState == GameState.MainMenuLoading) return;

            float clampedDelay = Mathf.Clamp(delay, MinLoadDelay, MaxLoadDelay);

            CurrentState = GameState.MainMenuLoading;
            MainMenuLoadingStarted?.Invoke(this, new(clampedDelay));

            await UniTask.WaitForSeconds(clampedDelay);

            CurrentState = GameState.MainMenu;
            LoadedLevel = 0;
            MainMenuLoaded?.Invoke(this, EventArgs.Empty);
        }

        public async UniTask LoadLevelAsync(int level, float delay)
        {
            if (level < 1)
                throw new ArgumentOutOfRangeException();

            if (LoadedLevel == level) return;

            float clampedDelay = Mathf.Clamp(delay, MinLoadDelay, MaxLoadDelay);

            CurrentState = GameState.LevelLoading;
            LevelLoadingStarted?.Invoke(this, new(level, clampedDelay));

            await UniTask.WaitForSeconds(clampedDelay);

            CurrentState = GameState.Level;
            LoadedLevel = level;
            LevelLoaded?.Invoke(this, new(level));
        }
    }
}