using SpaceAce.Main.GameStates;

using System;

using UnityEngine;

using Zenject;

namespace SpaceAce.Main.MasterCamera
{
    public sealed class MasterAudioListenerHolder : IInitializable, IDisposable
    {
        private readonly GameStateLoader _gameStateLoader;

        public AudioListener MasterAudioListener { get; private set; }

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
            _gameStateLoader.MainMenuLoaded += MainMenuLoadedEventHandler;
        }

        public void Dispose()
        {
            _gameStateLoader.MainMenuLoaded -= MainMenuLoadedEventHandler;
        }

        #endregion

        #region event handlers

        private void MainMenuLoadedEventHandler(object sender, EventArgs e)
        {
            MasterAudioListener.enabled = true;
        }

        #endregion
    }
}