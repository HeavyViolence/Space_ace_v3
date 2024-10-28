using Cysharp.Threading.Tasks;

using Newtonsoft.Json;
using SpaceAce.Auxiliary;
using SpaceAce.Auxiliary.EventArguments;
using SpaceAce.Main.GamePause;
using SpaceAce.Main.Saving;

using System;
using System.Threading;

using UnityEngine;

using Zenject;

namespace SpaceAce.Main.Audio
{
    public abstract class MusicPlayer : IInitializable, IDisposable, ISavable
    {
        public event EventHandler SavingRequested;
        public event EventHandler<ErrorOccurredEventArgs> ErrorOccurred;

        private readonly AudioCollection _music;
        private readonly AudioPlayer _audioPlayer;
        private readonly SavingSystem _savingSystem;
        private readonly GamePauser _gamePauser;

        protected CancellationTokenSource MusicCancellation;

        public bool IsPlaying { get; private set; } = false;
        public string SavedDataName => "Music settings";

        private MusicPlayerSettings _settings;

        public MusicPlayerSettings Settings
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

        public MusicPlayer(AudioCollection music,
                           AudioPlayer audioPlayer,
                           SavingSystem savingSystem,
                           GamePauser gamePauser)
        {
            _music = music == null ? throw new ArgumentNullException() : music;
            _audioPlayer = audioPlayer ?? throw new ArgumentNullException();
            _savingSystem = savingSystem ?? throw new ArgumentNullException();
            _gamePauser = gamePauser ?? throw new ArgumentNullException();
        }

        protected async UniTask PlayMusicForeverAsync(CancellationToken token)
        {
            IsPlaying = true;

            await AuxAsync.DelayAsync(() => Settings.FirstPlaybackDelay, () => _gamePauser.Paused == true, token);
            await _audioPlayer.PlayAsync(_music.NonRepeatingRandom, null, Vector3.zero, true, false, token);

            while (token.IsCancellationRequested == false)
            {
                await AuxAsync.DelayAsync(() => Settings.PlaybackDelay, () => _gamePauser.Paused == true, token);
                await _audioPlayer.PlayAsync(_music.NonRepeatingRandom, null, Vector3.zero, true, false, token);
            }

            IsPlaying = false;
        }

        #region interfaces

        public virtual void Initialize()
        {
            _savingSystem.Register(this, true);
        }

        public virtual void Dispose()
        {
            _savingSystem.Deregister(this);
        }

        public string GetState() =>
            JsonConvert.SerializeObject(Settings, Formatting.Indented);

        public void SetState(string state)
        {
            try
            {
                _settings = JsonConvert.DeserializeObject<MusicPlayerSettings>(state);
            }
            catch (Exception ex)
            {
                SetDefaultState();
                ErrorOccurred?.Invoke(this, new(ex));
            }
        }

        public void SetDefaultState() => _settings = MusicPlayerSettings.Default;

        public override bool Equals(object obj) =>
            obj is not null && Equals(obj as ISavable);

        public bool Equals(ISavable other) =>
            other is not null && SavedDataName == other.SavedDataName;

        public override int GetHashCode() => SavedDataName.GetHashCode();

        #endregion
    }
}