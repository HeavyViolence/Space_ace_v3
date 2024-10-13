using SpaceAce.Auxiliary;
using SpaceAce.Main.GamePause;
using SpaceAce.Main.GameStates;

using System;

using UnityEngine;

using Zenject;

namespace SpaceAce.Main.SpaceBackgrounds
{
    public sealed class SpaceBackground : IInitializable, IDisposable, ITickable
    {
        private readonly SpaceBackgroundConfig _config;
        private readonly GamePauser _gamePauser;
        private readonly GameStateLoader _gameStateLoader;
        private readonly MeshRenderer _renderer;
        private readonly ParticleSystem _spaceDust;

        public Vector2 ScrollVelocity { get; private set; } = Vector2.zero;

        public SpaceBackground(GameObject prefab,
                               SpaceBackgroundConfig config,
                               GamePauser gamePauser,
                               GameStateLoader gameStateLoader)
        {
            _config = config == null ? throw new ArgumentNullException() : config;
            _gamePauser = gamePauser ?? throw new ArgumentNullException();
            _gameStateLoader = gameStateLoader ?? throw new ArgumentNullException();

            GameObject spaceBackground = prefab == null ? throw new ArgumentNullException()
                                                        : UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);

            _renderer = spaceBackground.GetComponentInChildren<MeshRenderer>();

            if (_renderer == null)
            {
                throw new MissingComponentException($"Space background prefab is missing {typeof(MeshRenderer)}!");
            }

            _spaceDust = spaceBackground.GetComponentInChildren<ParticleSystem>();

            if (_spaceDust == null)
            {
                throw new MissingComponentException($"Space background prefab is missing space dust {typeof(ParticleSystem)}!");
            }

            SetMainMenuState();
        }

        public void SetMainMenuState()
        {
            _renderer.sharedMaterial = _config.GetRandomMainMenuBackground();
            _renderer.sharedMaterial.mainTextureOffset = new Vector2(0f, MyMath.RandomUnit);

            _spaceDust.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            ScrollVelocity = new(0f, _config.MainMenuBackgroundScrollSpeed.Random);
        }

        public void SetLevelState()
        {
            _renderer.sharedMaterial = _config.GetRandomSpaceBackground();
            _renderer.sharedMaterial.mainTextureOffset = new Vector2(0f, MyMath.RandomUnit);

            _spaceDust.Play(true);

            ScrollVelocity = new(0f, _config.SpaceBackgroundScrollSpeed.Random);
        }

        #region interfaces

        public void Initialize()
        {
            _gameStateLoader.MainMenuLoaded += MainMenuLoadedEventHandler;
            _gameStateLoader.LevelLoaded += LevelLoadedEventHandler;

            _gamePauser.GamePaused += GamePausedEventHandler;
            _gamePauser.GameResumed += GameResumedEventHandler;
        }

        public void Dispose()
        {
            _gameStateLoader.MainMenuLoaded -= MainMenuLoadedEventHandler;
            _gameStateLoader.LevelLoaded -= LevelLoadedEventHandler;

            _gamePauser.GamePaused -= GamePausedEventHandler;
            _gamePauser.GameResumed -= GameResumedEventHandler;
        }

        public void Tick()
        {
            if (_gamePauser.Paused == true)
            {
                return;
            }

            _renderer.sharedMaterial.mainTextureOffset += ScrollVelocity * Time.deltaTime;
        }

        #endregion

        #region event handlers

        private void MainMenuLoadedEventHandler(object sender, EventArgs e)
        {
            SetMainMenuState();
        }

        private void LevelLoadedEventHandler(object sender, LevelLoadedEventArgs e)
        {
            SetLevelState();
        }

        private void GamePausedEventHandler(object sender, EventArgs e)
        {
            _spaceDust.Pause(true);
        }

        private void GameResumedEventHandler(object sender, EventArgs e)
        {
            _spaceDust.Play(true);
        }

        #endregion
    }
}