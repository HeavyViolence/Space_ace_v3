using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using NaughtyAttributes;

using SpaceAce.Auxiliary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace SpaceAce.Main.Audio
{
    [CreateAssetMenu(fileName = "Audio collection",
                     menuName = "Space ace/Configs/Audio/Audio collection")]
    public sealed class AudioCollection : ScriptableObject
    {
        public const float MinVolume = 0f;
        public const float MaxVolume = 1f;

        public const float MinSpatialBlend = 0f;
        public const float MaxSpatialBlend = 1f;
        public const float DefaultSpatialBlend = 0.5f;

        public const float MinPitch = 0.5f;
        public const float MaxPitch = 2f;
        public const float DefaultPitch = 1f;

        private int _nextAudioClipIndex = 0;
        private IEnumerator<AudioClip> _shuffledAudio = null;

        [SerializeField]
        private List<AudioClip> _audioClips;

        [SerializeField]
        private AudioMixerGroup _outputAudioGroup;

        [SerializeField, MinMaxSlider(MinVolume, MaxVolume)]
        private Vector2 _volume = new(MinVolume, MaxVolume);

        [SerializeField]
        private AudioPriority _priority = AudioPriority.Lowest;

        [SerializeField, MinMaxSlider(MinSpatialBlend, MaxSpatialBlend)]
        private Vector2 _spatialBlend = new(MinSpatialBlend, MaxSpatialBlend);

        [SerializeField, MinMaxSlider(MinPitch, MaxPitch)]
        private Vector2 _pitch = new(MinPitch, MaxPitch);

        public int AudioClipsAmount => _audioClips.Count;

        public AudioProperties Next => new(NextAudio,
                                           _outputAudioGroup,
                                           RandomVolume,
                                           _priority,
                                           RandomSpatialBlend,
                                           RandomPitch);

        public AudioProperties Random => new(RandomAudio,
                                             _outputAudioGroup,
                                             RandomVolume,
                                             _priority,
                                             RandomSpatialBlend,
                                             RandomPitch);

        public AudioProperties NonRepeatingRandom => new(ShuffledAudio,
                                                         _outputAudioGroup,
                                                         RandomVolume,
                                                         _priority,
                                                         RandomSpatialBlend,
                                                         RandomPitch);

        private float RandomVolume => UnityEngine.Random.Range(_volume.x, _volume.y);

        private float RandomSpatialBlend => UnityEngine.Random.Range(_spatialBlend.x, _spatialBlend.y);

        private float RandomPitch => UnityEngine.Random.Range(_pitch.x, _pitch.y);

        private AudioClip NextAudio => _audioClips[_nextAudioClipIndex++ % _audioClips.Count];

        private AudioClip RandomAudio => MyMath.GetRandom(_audioClips);

        private AudioClip ShuffledAudio
        {
            get
            {
                _shuffledAudio ??= MyMath.Shuffle(_audioClips).GetEnumerator();

                if (_shuffledAudio.MoveNext() == false)
                {
                    _shuffledAudio = MyMath.Shuffle(_audioClips).GetEnumerator();
                    _shuffledAudio.MoveNext();
                }

                return _shuffledAudio.Current;
            }
        }

        #region audio preview

        private readonly HashSet<GameObject> _previews = new();

        [Button("Preview audio")]
        private async UniTaskVoid PrivewAudioAsync()
        {
            if (AudioClipsAmount == 0)
            {
                return;
            }

            AudioClip audio = ShuffledAudio;

            GameObject preview = EditorUtility.CreateGameObjectWithHideFlags($"Audio preview: {audio.name}", HideFlags.None);
            AudioSource source = preview.AddComponent<AudioSource>();

            source.clip = audio;
            source.spatialBlend = RandomSpatialBlend;
            source.pitch = RandomPitch;
            source.volume = RandomVolume;
            source.outputAudioMixerGroup = _outputAudioGroup;

            source.Play();

            _previews.Add(preview);

            await UniTask.WaitForSeconds(audio.length * source.pitch);

            if (_previews.Contains(preview) == true)
            {
                _previews.Remove(preview);
                DestroyImmediate(preview);
            }
        }

        [Button("Force stop")]
        private void ForceStop()
        {
            if (_previews.Count == 0)
            {
                return;
            }

            foreach (GameObject preview in _previews)
            {
                DestroyImmediate(preview);
            }

            _previews.Clear();
        }

        #endregion
    }
}