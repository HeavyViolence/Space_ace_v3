using Cysharp.Threading.Tasks;

using SpaceAce.Main.GameStates;

using System;
using System.Threading;

using VContainer;
using VContainer.Unity;

namespace SpaceAce.Main.Audio
{
    public sealed class MusicPlayer : IInitializable, IDisposable
    {
        private readonly MusicPlayerConfig _config;
        private readonly AudioPlayer _audioPlayer;
        private readonly GameStateLoader _gameStateLoader;

        private CancellationTokenSource _audioCancellation;

        [Inject]
        public MusicPlayer(MusicPlayerConfig config,
                           AudioPlayer audioPlayer,
                           GameStateLoader gameStateLoader)
        {
            _config = config == null ? throw new ArgumentNullException() : config;
            _audioPlayer = audioPlayer ?? throw new ArgumentNullException();
            _gameStateLoader = gameStateLoader ?? throw new ArgumentNullException();
        }

        private async UniTask PlayMainMenuMusicForeverAsync(CancellationToken token)
        {
            await UniTask.WaitForSeconds(_config.MainMenuPlaybackDelay.Random, true, PlayerLoopTiming.Update, token);

            while (token.IsCancellationRequested == false)
            {
                await _audioPlayer.PlayAsync(_config.MainMenuAudio.Random, null, UnityEngine.Vector3.zero, true, false, token);
                await UniTask.WaitForSeconds(_config.MainMenuPlaybackDelay.Random, true, PlayerLoopTiming.Update, token);
            }
        }

        private async UniTask PlayBattleMusicForeverAsync(CancellationToken token)
        {
            await UniTask.WaitForSeconds(_config.BattlePlaybackDelay.Random, true, PlayerLoopTiming.Update, token);

            while (token.IsCancellationRequested == false)
            {
                await _audioPlayer.PlayAsync(_config.BattleAudio.Random, null, UnityEngine.Vector3.zero, true, false, token);
                await UniTask.WaitForSeconds(_config.BattlePlaybackDelay.Random, true, PlayerLoopTiming.Update, token);
            }
        }

        #region interfaces

        public void Initialize()
        {
            _gameStateLoader.MainMenuLoadingStarted += OnMainMenuLoadingStarted;
            _gameStateLoader.MainMenuLoaded += OnMainMenuLoaded;

            _gameStateLoader.BattleStateLoadingStarted += OnBattleStateLoadingStarted;
            _gameStateLoader.BattleStateLoaded += OnBattleStateLoaded;

            OnInitialize();
        }

        public void Dispose()
        {
            _gameStateLoader.MainMenuLoadingStarted -= OnMainMenuLoadingStarted;
            _gameStateLoader.MainMenuLoaded -= OnMainMenuLoaded;

            _gameStateLoader.BattleStateLoadingStarted -= OnBattleStateLoadingStarted;
            _gameStateLoader.BattleStateLoaded -= OnBattleStateLoaded;
        }

        #endregion

        #region event handlers

        private void OnInitialize()
        {
            _audioCancellation = new();
            PlayMainMenuMusicForeverAsync(_audioCancellation.Token).Forget();
        }

        private void OnMainMenuLoadingStarted(MainMenuLoadingStartedArgs e)
        {
            _audioCancellation?.Cancel();
            _audioCancellation?.Dispose();
        }

        private void OnMainMenuLoaded()
        {
            _audioCancellation = new();
            PlayMainMenuMusicForeverAsync(_audioCancellation.Token).Forget();
        }

        private void OnBattleStateLoadingStarted(BattleStateLoadingStartedArgs e)
        {
            _audioCancellation?.Cancel();
            _audioCancellation?.Dispose();
        }

        private void OnBattleStateLoaded(BattleDifficulty difficulty)
        {
            _audioCancellation = new();
            PlayBattleMusicForeverAsync(_audioCancellation.Token).Forget();
        }

        #endregion
    }
}