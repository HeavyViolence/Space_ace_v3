using Cysharp.Threading.Tasks;

using Newtonsoft.Json;

using SpaceAce.Auxiliary.EventArguments;
using SpaceAce.Main.GamePause;
using SpaceAce.Main.Saving;

using System;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.Audio;

using Zenject;

namespace SpaceAce.Main.Audio
{
    public sealed class AudioPlayer : IInitializable, IDisposable, ISavable
    {
        public const int MinAudioSources = 16;
        public const int MaxAudioSources = 256;

        public event EventHandler SavingRequested;
        public event EventHandler<ErrorOccurredEventArgs> ErrorOccurred;

        private readonly AudioMixer _audioMixer;
        private readonly SavingSystem _savingSystem;
        private readonly GamePauser _gamePauser;

        private readonly Transform _audioSourcesAnchor = new GameObject("Audio sources anchor").transform;
        private readonly Dictionary<Guid, AudioSourceCache> _activeAudioSources;
        private readonly Stack<AudioSourceCache> _availableAudioSources;

        public int AudioSources { get; }
        public string SavedDataName => "Audio settings";

        private AudioPlayerSettings _settings = AudioPlayerSettings.Default;

        public AudioPlayerSettings Settings
        {
            get => _settings;

            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException();
                }

                _settings = value;

                _audioMixer.SetFloat("Master volume", value.MasterVolume);
                _audioMixer.SetFloat("Shooting volume", value.ShootingVolume);
                _audioMixer.SetFloat("Explosions volume", value.ExplosionsVolume);
                _audioMixer.SetFloat("Background volume", value.BackgroundVolume);
                _audioMixer.SetFloat("Interface volume", value.InterfaceVolume);
                _audioMixer.SetFloat("Music volume", value.MusicVolume);
                _audioMixer.SetFloat("Interactions volume", value.InteractionsVolume);
                _audioMixer.SetFloat("Notifications volume", value.NotificationsVolume);

                SavingRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        public AudioPlayer(int audioSources, AudioMixer mixer, SavingSystem savingSystem, GamePauser gamePauser)
        {
            AudioSources = Mathf.Clamp(audioSources, MinAudioSources, MaxAudioSources);
            _audioMixer = mixer == null ? throw new ArgumentNullException() : mixer;
            _savingSystem = savingSystem ?? throw new ArgumentNullException();
            _gamePauser = gamePauser ?? throw new ArgumentNullException();

            _activeAudioSources = new(AudioSources);
            _availableAudioSources = new(AudioSources);

            CreateAudioSourcePool();
        }

        public async UniTask PlayAsync(AudioProperties properties,
                                       Transform anchor,
                                       Vector3 position,
                                       bool obeyGamePause,
                                       bool loop,
                                       CancellationToken token = default)
        {
            AudioSourceCache cache = FindAvailableAudioSourceCache();
            AudioAccess access = ConfigureAudioSourceCache(cache, properties, anchor, position, loop);

            float timer = 0f;

            while (loop == true || timer < access.PlaybackDuration)
            {
                if (token.IsCancellationRequested == true)
                {
                    break;
                }

                timer += Time.deltaTime;

                if (obeyGamePause == true && _gamePauser.Paused == true)
                {
                    cache.AudioSource.Pause();

                    while (_gamePauser.Paused == true)
                    {
                        await UniTask.Yield();
                    }

                    cache.AudioSource.Play();
                }

                await UniTask.Yield();
            }

            DisableActiveAudioSource(access.ID);
        }

        private void CreateAudioSourcePool()
        {
            for (int i = 0; i < AudioSources; i++)
            {
                var audioSourceHolder = new GameObject($"Audio source #{i + 1}");
                audioSourceHolder.transform.parent = _audioSourcesAnchor;

                var audioSource = audioSourceHolder.AddComponent<AudioSource>();
                AudioSourceCache cache = new(audioSource, audioSourceHolder.transform);

                ResetAudioSourceCache(cache);
                _availableAudioSources.Push(cache);
            }
        }

        private bool DisableActiveAudioSource(Guid id)
        {
            if (_activeAudioSources.TryGetValue(id, out AudioSourceCache cache) == true)
            {
                ResetAudioSourceCache(cache);

                _activeAudioSources.Remove(id);
                _availableAudioSources.Push(cache);

                return true;
            }

            return false;
        }

        private AudioSourceCache FindAvailableAudioSourceCache()
        {
            if (_availableAudioSources.Count > 0)
            {
                return _availableAudioSources.Pop();
            }

            byte priority = 0;
            Guid id = Guid.Empty;
            AudioSourceCache availableSource = null;

            foreach (var entry in _activeAudioSources)
            {
                if (entry.Value.AudioSource.loop == true)
                {
                    continue;
                }

                if (entry.Value.AudioSource.priority > priority)
                {
                    priority = (byte)entry.Value.AudioSource.priority;
                    id = entry.Key;
                    availableSource = entry.Value;
                }
            }

            _activeAudioSources.Remove(id);
            ResetAudioSourceCache(availableSource);

            return availableSource;
        }

        private void ResetAudioSourceCache(AudioSourceCache cache)
        {
            if (cache is null)
            {
                throw new ArgumentNullException();
            }

            if (cache.AudioSource == null || cache.Transform == null)
            {
                return;
            }

            cache.AudioSource.Stop();

            cache.AudioSource.clip = null;
            cache.AudioSource.outputAudioMixerGroup = null;
            cache.AudioSource.mute = true;
            cache.AudioSource.bypassEffects = true;
            cache.AudioSource.bypassListenerEffects = true;
            cache.AudioSource.bypassReverbZones = true;
            cache.AudioSource.playOnAwake = false;
            cache.AudioSource.loop = false;
            cache.AudioSource.priority = byte.MaxValue;
            cache.AudioSource.volume = 0f;
            cache.AudioSource.spatialBlend = 0f;
            cache.AudioSource.pitch = 1f;
            cache.AudioSource.reverbZoneMix = 0f;

            cache.Transform.parent = _audioSourcesAnchor.transform;
            cache.Transform.position = Vector3.zero;
            cache.Transform.gameObject.SetActive(false);
        }

        private AudioAccess ConfigureAudioSourceCache(AudioSourceCache cache,
                                                      AudioProperties properties,
                                                      Transform anchor,
                                                      Vector3 position,
                                                      bool loop)
        {
            AudioAccess access;
            var id = Guid.NewGuid();

            cache.AudioSource.clip = properties.Clip;
            cache.AudioSource.outputAudioMixerGroup = properties.OutputAudioGroup;
            cache.AudioSource.mute = false;
            cache.AudioSource.bypassEffects = false;
            cache.AudioSource.bypassListenerEffects = false;
            cache.AudioSource.bypassReverbZones = false;
            cache.AudioSource.priority = (int)properties.Priority;
            cache.AudioSource.volume = properties.Volume;
            cache.AudioSource.spatialBlend = properties.SpatialBlend;
            cache.AudioSource.pitch = properties.Pitch;

            if (anchor != null)
            {
                cache.Transform.parent = anchor;
            }

            if (loop == true)
            {
                cache.AudioSource.loop = true;
                access = new AudioAccess(id, float.PositiveInfinity);
            }
            else
            {
                cache.AudioSource.loop = false;
                access = new AudioAccess(id, properties.Clip.length);
            }

            cache.Transform.localPosition = position;
            cache.Transform.gameObject.SetActive(true);
            cache.AudioSource.Play();

            _activeAudioSources.Add(id, cache);

            return access;
        }

        #region interfaces

        public void Initialize()
        {
            _savingSystem.Register(this);
        }

        public void Dispose()
        {
            _savingSystem.Deregister(this);
        }

        public string GetState() =>
            JsonConvert.SerializeObject(Settings, Formatting.Indented);

        public void SetState(string state)
        {
            try
            {
                _settings = JsonConvert.DeserializeObject<AudioPlayerSettings>(state);
            }
            catch (Exception ex)
            {
                _settings = AudioPlayerSettings.Default;
                ErrorOccurred?.Invoke(this, new(ex));
            }
        }

        public override bool Equals(object obj) =>
            obj is not null && Equals(obj as ISavable);

        public bool Equals(ISavable other) =>
            other is not null && SavedDataName == other.SavedDataName;

        public override int GetHashCode() => SavedDataName.GetHashCode();

        #endregion
    }
}