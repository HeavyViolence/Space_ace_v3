using Cysharp.Threading.Tasks;

using SpaceAce.Main.Localization;

using System;

using UnityEngine;

namespace SpaceAce.GUI
{
    public sealed class MainMenu : GUIPanel
    {
        public event EventHandler PlayButtonClicked,
                                  InventoryButtonClicked,
                                  ArmoryButtonClicked,
                                  SettingsButtonClicked,
                                  StatisticsButtonClicked,
                                  AuthorizationButtonClicked,
                                  CheatsButtonClicked;

        #region setup

        [SerializeField]
        private ButtonCache _playButton;

        [SerializeField]
        private ButtonCache _inventoryButton;

        [SerializeField]
        private ButtonCache _armoryButton;

        [SerializeField]
        private ButtonCache _settingsButton;

        [SerializeField]
        private ButtonCache _statisticsButton;

        [SerializeField]
        private ButtonCache _authorizationButton;

        [SerializeField]
        private ButtonCache _cheatsButton;

        #endregion

        public override async UniTask LocalizeAsync(Localizer localizer)
        {
            _playButton.TextMesh.text = await localizer.GetLocalizedStringAsync("Main menu", "Play");
            _inventoryButton.TextMesh.text = await localizer.GetLocalizedStringAsync("Main menu", "Inventory");
            _armoryButton.TextMesh.text = await localizer.GetLocalizedStringAsync("Main menu", "Armory");
            _settingsButton.TextMesh.text = await localizer.GetLocalizedStringAsync("Main menu", "Settings");
            _statisticsButton.TextMesh.text = await localizer.GetLocalizedStringAsync("Main menu", "Statistics");
            _authorizationButton.TextMesh.text = await localizer.GetLocalizedStringAsync("Main menu", "Authorization");
            _cheatsButton.TextMesh.text = await localizer.GetLocalizedStringAsync("Main menu", "Cheats");
        }

        protected override void OnBind()
        {
            _playButton.Button.onClick.AddListener(OnPlayButtonClicked);
            _playButton.HoveredOver += OnHoveredOverButton;

            _inventoryButton.Button.onClick.AddListener(OnInventoryButtonClicked);
            _inventoryButton.HoveredOver += OnHoveredOverButton;

            _armoryButton.Button.onClick.AddListener(OnArmoryButtonClicked);
            _armoryButton.HoveredOver += OnHoveredOverButton;

            _settingsButton.Button.onClick.AddListener(OnSettingsButtonClicked);
            _settingsButton.HoveredOver += OnHoveredOverButton;

            _statisticsButton.Button.onClick.AddListener(OnStatisticsButtonClicked);
            _statisticsButton.HoveredOver += OnHoveredOverButton;

            _authorizationButton.Button.onClick.AddListener(OnAuthorizationButtonCkicked);
            _authorizationButton.HoveredOver += OnHoveredOverButton;

            _cheatsButton.Button.onClick.AddListener(OnCheatsButtonClicked);
            _cheatsButton.HoveredOver += OnHoveredOverButton;
        }

        protected override void OnClear()
        {
            _playButton.Button.onClick.RemoveAllListeners();
            _playButton.HoveredOver -= OnHoveredOverButton;

            _inventoryButton.Button.onClick.RemoveAllListeners();
            _inventoryButton.HoveredOver -= OnHoveredOverButton;

            _armoryButton.Button.onClick.RemoveAllListeners();
            _armoryButton.HoveredOver -= OnHoveredOverButton;

            _settingsButton.Button.onClick.RemoveAllListeners();
            _settingsButton.HoveredOver -= OnHoveredOverButton;

            _statisticsButton.Button.onClick.RemoveAllListeners();
            _statisticsButton.HoveredOver -= OnHoveredOverButton;

            _authorizationButton.Button.onClick.RemoveAllListeners();
            _authorizationButton.HoveredOver -= OnHoveredOverButton;

            _cheatsButton.Button.onClick.RemoveAllListeners();
            _cheatsButton.HoveredOver -= OnHoveredOverButton;
        }

        protected async override UniTask TweenOnEnableAsync()
        {
            await UniTask.Yield();
            throw new NotImplementedException();
        }

        protected async override UniTask TweenOnDisableAsync()
        {
            await UniTask.Yield();
            throw new NotImplementedException();
        }

        #region buttons

        private void OnPlayButtonClicked()
        {
            PlayButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnInventoryButtonClicked()
        {
            InventoryButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnArmoryButtonClicked()
        {
            ArmoryButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnSettingsButtonClicked()
        {
            SettingsButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnStatisticsButtonClicked()
        {
            StatisticsButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnAuthorizationButtonCkicked()
        {
            AuthorizationButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnCheatsButtonClicked()
        {
            CheatsButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}