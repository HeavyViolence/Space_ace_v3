using Cysharp.Threading.Tasks;

using System;

using UnityEngine;

namespace SpaceAce.Main.GameStates
{
    public sealed class GameStateLoader
    {
        public const float MinLoadDelay = 0f;
        public const float MaxLoadDelay = 10f;

        public event Action<MainMenuLoadingStartedArgs> MainMenuLoadingStarted;
        public event Action MainMenuLoaded;

        public event Action<BattleStateLoadingStartedArgs> BattleStateLoadingStarted;
        public event Action<BattleDifficulty> BattleStateLoaded;

        public GameState CurrentState { get; private set; } = GameState.MainMenu;

        public async UniTask LoadMainMenuAsync(float delay)
        {
            if (CurrentState == GameState.MainMenu ||
                CurrentState == GameState.MainMenuLoading)
            {
                return;
            }

            CurrentState = GameState.MainMenuLoading;

            float clampedDelay = Mathf.Clamp(delay, MinLoadDelay, MaxLoadDelay);
            MainMenuLoadingStarted?.Invoke(new(clampedDelay));

            await UniTask.WaitForSeconds(clampedDelay);

            CurrentState = GameState.MainMenu;
            MainMenuLoaded?.Invoke();
        }

        public async UniTask LoadBattleAsync(BattleDifficulty difficulty, float delay)
        {
            if (CurrentState == GameState.Battle ||
                CurrentState == GameState.BattleLoading)
            {
                return;
            }

            CurrentState = GameState.BattleLoading;

            float clampedDelay = Mathf.Clamp(delay, MinLoadDelay, MaxLoadDelay);
            BattleStateLoadingStarted?.Invoke(new(difficulty, clampedDelay));

            await UniTask.WaitForSeconds(clampedDelay);

            CurrentState = GameState.Battle;
            BattleStateLoaded?.Invoke(difficulty);
        }
    }
}