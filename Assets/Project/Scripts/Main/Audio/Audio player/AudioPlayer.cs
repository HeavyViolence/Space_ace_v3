using Cysharp.Threading.Tasks;

using MessagePack;

using SpaceAce.Auxiliary.Easing;
using SpaceAce.Main.GamePause;
using SpaceAce.Main.Saving;

using System;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.Audio;

using VContainer;
using VContainer.Unity;

namespace SpaceAce.Main.Audio
{
    public sealed class AudioPlayer : IInitializable, IDisposable, ISavable
    {
        public const int MinAudioSources = 16;
        public const int MaxAudioSources = 256;

        public event Action StateChanged;

        private readonly AudioMixer _audioMixer;
        private readonly SavingSystem _savingSystem;
        private readonly GamePauser _gamePauser;
        private readonly EasingService _easingService;

        private readonly Transform _audioSourcesAnchor = new GameObject("Audio sources anchor").transform;
        private readonly Dictionary<Guid, AudioSourceCache> _activeAudioSources;
        private readonly Stack<AudioSourceCache> _availableAudioSources;

        public int AudioSources { get; }
        public string StateName => "Audio settings";

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

                _audioMixer.SetFloat("Master volumeFactor", value.MasterVolume);
                _audioMixer.SetFloat("Shooting volumeFactor", value.ShootingVolume);
                _audioMixer.SetFloat("Explosions volumeFactor", value.ExplosionsVolume);
                _audioMixer.SetFloat("Background volumeFactor", value.BackgroundVolume);
                _audioMixer.SetFloat("Interface volumeFactor", value.InterfaceVolume);
                _audioMixer.SetFloat("MusicPlayerConfig volumeFactor", value.MusicVolume);
                _audioMixer.SetFloat("Interactions volumeFactor", value.InteractionsVolume);
                _audioMixer.SetFloat("Notifications volumeFactor", value.NotificationsVolume);

                StateChanged?.Invoke();
            }
        }

        [Inject]
        public AudioPlayer(int audioSources,
                           AudioMixer mixer,
                           SavingSystem savingSystem,
                           GamePauser gamePauser,
                           EasingService easingService)
        {
            AudioSources = Mathf.Clamp(audioSources, MinAudioSources, MaxAudioSources);
            _audioMixer = mixer == null ? throw new ArgumentNullException() : mixer;
            _savingSystem = savingSystem ?? throw new ArgumentNullException();
            _gamePauser = gamePauser ?? throw new ArgumentNullException();
            _easingService = easingService ?? throw new ArgumentNullException();

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
                    if (properties.CancelWithEasing == true)
                    {
                        float cancellationTimer = 0f;
                        float initialVolume = cache.AudioSource.volume;

                        while (cancellationTimer < properties.CancellationDuration)
                        {
                            cancellationTimer += Time.deltaTime;
                            float t = cancellationTimer / properties.CancellationDuration;
                            float volumeFactor = _easingService.Ease(1f, 0f, t, properties.CancellationEasing);
                            float volume = initialVolume * volumeFactor;

                            cache.AudioSource.volume = volume;

                            await UniTask.Yield();
                        }
                    }

                    break;
                }

                if (obeyGamePause == true && _gamePauser.Paused == true)
                {
                    cache.AudioSource.Pause();
                    await UniTask.WaitUntil(() => _gamePauser.Paused == false, PlayerLoopTiming.Update, token);
                    cache.AudioSource.Play();
                }

                timer += Time.deltaTime;

                if (properties.PlayWithEasing == true && cache.AudioSource != null)
                {
                    float t = timer / access.PlaybackDuration;
                    float volumeFactor = _easingService.Ease(0f, 1f, t, EasingMode.FlatFastInOut);
                    float volume = properties.Volume * volumeFactor;

                    cache.AudioSource.volume = volume;
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
                AudioSourceCache cache = new(audioSource, audioSourceHolder.transform, AudioPriority.Lowest);

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

            AudioPriority priority = AudioPriority.Default;
            Guid id = Guid.Empty;
            AudioSourceCache availableSource = null;

            foreach (var entry in _activeAudioSources)
            {
                if (entry.Value.AudioSource.loop == true)
                {
                    continue;
                }

                if (entry.Value.Priority <= priority)
                {
                    priority = entry.Value.Priority;
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
            cache.AudioSource.volume = 0f;
            cache.AudioSource.spatialBlend = 0f;
            cache.AudioSource.pitch = 1f;
            cache.AudioSource.reverbZoneMix = 0f;

            cache.Transform.parent = _audioSourcesAnchor.transform;
            cache.Transform.position = Vector3.zero;
            cache.Transform.gameObject.SetActive(false);

            cache.Priority = AudioPriority.Lowest;
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
                access = new AudioAccess(id, properties.Clip.length * properties.Pitch);
            }

            cache.Transform.localPosition = position;
            cache.Transform.gameObject.SetActive(true);

            cache.Priority = properties.Priority;

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

        public byte[] GetState() => MessagePackSerializer.Serialize(Settings);

        public void SetState(byte[] state)
        {
            try
            {
                _settings = MessagePackSerializer.Deserialize<AudioPlayerSettings>(state);
            }
            catch (Exception)
            {
                _settings = AudioPlayerSettings.Default;
            }
        }

        #endregion
    }
}