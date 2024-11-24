using Cysharp.Threading.Tasks;

using Newtonsoft.Json;

using SpaceAce.Auxiliary.EventArguments;
using SpaceAce.Main.GameStates;
using SpaceAce.Main.Saving;

using System;
using System.Threading;

using Zenject;

namespace SpaceAce.Main.Audio
{
    public sealed class BackgroundAudioPlayer : IInitializable, IDisposable, ISavable
    {
        public event EventHandler SavingRequested;
        public event EventHandler<ErrorOccurredEventArgs> ErrorOccurred;

        private readonly BackgroundAudio _audio;
        private readonly AudioPlayer _audioPlayer;
        private readonly GameStateLoader _gameStateLoader;
        private readonly SavingSystem _savingSystem;

        private CancellationTokenSource _audioCancellation;

        public string SavedDataName => "Background audio settings";

        private BackgroundAudioPlayerSettings _settings = BackgroundAudioPlayerSettings.Default;
        public BackgroundAudioPlayerSettings Settings
        {
            get => _settings;

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                _settings = value;
                SavingRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        public BackgroundAudioPlayer(BackgroundAudio audio,
                                     AudioPlayer audioPlayer,
                                     GameStateLoader gameStateLoader,
                                     SavingSystem savingSystem)
        {
            _audio = audio == null ? throw new ArgumentNullException() : audio;
            _audioPlayer = audioPlayer ?? throw new ArgumentNullException();
            _gameStateLoader = gameStateLoader ?? throw new ArgumentNullException();
            _savingSystem = savingSystem ?? throw new ArgumentNullException();
        }

        private async UniTask PlayMainMenuBackgroundAudioForeverAsync(CancellationToken token)
        {
            while (token.IsCancellationRequested == false)
            {
                AudioProperties audio = _audio.MainMenuAudio.Random;
                await _audioPlayer.PlayAsync(audio, null, UnityEngine.Vector3.zero, true, false, token);

                await UniTask.WaitForSeconds(Settings.MainMenuPlaybackDelay, true, PlayerLoopTiming.Update, token);
            }
        }

        private async UniTask PlayLevelBackgroundAudioForeverAsync(CancellationToken token)
        {
            float firstDelay = UnityEngine.Random.Range(0f, Settings.LevelFirstPlaybackDelay);
            await UniTask.WaitForSeconds(firstDelay, true, PlayerLoopTiming.Update, token);

            while (token.IsCancellationRequested == false)
            {
                AudioProperties audio = _audio.LevelAudio.Random;
                await _audioPlayer.PlayAsync(audio, null, UnityEngine.Vector3.zero, true, false, token);

                float delay = UnityEngine.Random.Range(0f, Settings.LevelPlaybackDelay);
                await UniTask.WaitForSeconds(delay, true, PlayerLoopTiming.Update, token);
            }
        }

        #region interfaces

        public void Initialize()
        {
            _savingSystem.Register(this);

            _gameStateLoader.MainMenuLoadingStarted += MainMenuLoadingStartedEventHandler;
            _gameStateLoader.MainMenuLoaded += MainMenuLoadedEventHandler;

            _gameStateLoader.LevelLoadingStarted += LevelLoadingStartedEventHandler;
            _gameStateLoader.LevelLoaded += LevelLoadedEventHandler;

            OnInitialize();
        }

        public void Dispose()
        {
            _savingSystem.Deregister(this);

            _gameStateLoader.MainMenuLoadingStarted -= MainMenuLoadingStartedEventHandler;
            _gameStateLoader.MainMenuLoaded -= MainMenuLoadedEventHandler;

            _gameStateLoader.LevelLoadingStarted -= LevelLoadingStartedEventHandler;
            _gameStateLoader.LevelLoaded -= LevelLoadedEventHandler;
        }

        public string GetState() =>
            JsonConvert.SerializeObject(Settings, Formatting.Indented);

        public void SetState(string state)
        {
            try
            {
                _settings = JsonConvert.DeserializeObject<BackgroundAudioPlayerSettings>(state);
            }
            catch (Exception ex)
            {
                _settings = BackgroundAudioPlayerSettings.Default;
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

        #region event handlers

        private void OnInitialize()
        {
            if (Settings.MainMenuBackgroundAudioEnabled == true)
            {
                _audioCancellation = new();
                PlayMainMenuBackgroundAudioForeverAsync(_audioCancellation.Token).Forget();
            }
        }

        private void MainMenuLoadingStartedEventHandler(object sender, MainMenuLoadingStartedEventArgs e)
        {
            _audioCancellation?.Cancel();
            _audioCancellation?.Dispose();
        }

        private void MainMenuLoadedEventHandler(object sender, EventArgs e)
        {
            if (Settings.MainMenuBackgroundAudioEnabled == true)
            {
                _audioCancellation = new();
                PlayMainMenuBackgroundAudioForeverAsync(_audioCancellation.Token).Forget();
            }
        }

        private void LevelLoadingStartedEventHandler(object sender, LevelLoadingStartedEventArgs e)
        {
            _audioCancellation?.Cancel();
            _audioCancellation?.Dispose();
        }

        private void LevelLoadedEventHandler(object sender, LevelLoadedEventArgs e)
        {
            if (Settings.LevelBackgroundAudioEnabled == true)
            {
                _audioCancellation = new();
                PlayLevelBackgroundAudioForeverAsync(_audioCancellation.Token).Forget();
            }
        }

        #endregion
    }
}