using SpaceAce.Main.GameStates;

using System;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SpaceAce.Main.MasterCamera
{
    public sealed class MasterAudioListenerHolder : IInitializable, IDisposable
    {
        private readonly GameStateLoader _gameStateLoader;

        public AudioListener MasterAudioListener { get; private set; }

        [Inject]
        public MasterAudioListenerHolder(MasterCameraHolder masterCameraHolder,
                                         GameStateLoader gameStateLoader)
        {
            MasterAudioListener = masterCameraHolder is null ? throw new ArgumentNullException()
                                                             : masterCameraHolder.MasterCameraAnchor.GetComponentInChildren<AudioListener>();

            if (MasterAudioListener == null)
            {
                throw new MissingComponentException($"Camera prefab is missing {typeof(AudioListener)}!");
            }

            MasterAudioListener.enabled = true;

            _gameStateLoader = gameStateLoader ?? throw new ArgumentNullException();
        }

        #region interfaces

        public void Initialize()
        {
            _gameStateLoader.MainMenuLoaded += OnMainMenuLoaded;
        }

        public void Dispose()
        {
            _gameStateLoader.MainMenuLoaded -= OnMainMenuLoaded;
        }

        #endregion

        #region event handlers

        private void OnMainMenuLoaded()
        {
            MasterAudioListener.enabled = true;
        }

        #endregion
    }
}