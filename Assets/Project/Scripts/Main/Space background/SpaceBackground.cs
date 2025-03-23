using SpaceAce.Auxiliary;
using SpaceAce.Main.GamePause;
using SpaceAce.Main.GameStates;

using System;

using UnityEngine;

using VContainer;
using VContainer.Unity;

namespace SpaceAce.Main.SpaceBackgrounds
{
    public sealed class SpaceBackground : IInitializable, IDisposable, ITickable
    {
        private readonly SpaceBackgroundConfig _config;
        private readonly GamePauser _gamePauser;
        private readonly GameStateLoader _gameStateLoader;
        private readonly MeshRenderer _renderer;

        public Vector2 ScrollVelocity { get; private set; } = Vector2.zero;

        [Inject]
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

            SetMainMenuState();
        }

        public void SetMainMenuState()
        {
            _renderer.sharedMaterial = _config.GetRandomBackground();
            _renderer.sharedMaterial.mainTextureOffset = MyMath.RandomUnit2D;

            ScrollVelocity = new(0f, _config.MainMenuScrollSpeed.Random);
        }

        public void SetBattleState()
        {
            _renderer.sharedMaterial = _config.GetRandomBackground();
            _renderer.sharedMaterial.mainTextureOffset = MyMath.RandomUnit2D;

            ScrollVelocity = new(0f, _config.LevelScrollSpeed.Random);
        }

        #region interfaces

        public void Initialize()
        {
            _gameStateLoader.MainMenuLoaded += OnMainMenuLoaded;
            _gameStateLoader.BattleStateLoaded += OnBattleStateLoaded;

            SetMainMenuState();
        }

        public void Dispose()
        {
            _gameStateLoader.MainMenuLoaded -= OnMainMenuLoaded;
            _gameStateLoader.BattleStateLoaded -= OnBattleStateLoaded;
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

        private void OnMainMenuLoaded()
        {
            SetMainMenuState();
        }

        private void OnBattleStateLoaded(BattleDifficulty difficulty)
        {
            SetBattleState();
        }

        #endregion
    }
}