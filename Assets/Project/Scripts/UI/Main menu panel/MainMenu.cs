using Cysharp.Threading.Tasks;

using System;

using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace SpaceAce.UI
{
    public sealed class MainMenu : UIPanel
    {
        public event EventHandler PlayButtonClicked,
                                  InventoryButtonClicked,
                                  ArmoryButtonClicked,
                                  SettingsButtonClicked,
                                  StatisticsButtonClicked,
                                  SavesButtonClicked,
                                  CommandsButtonClicked,
                                  InfoButtonClicked;

        public event EventHandler<HoveredOverButtonEventArgs> HoveredOverButton;

        protected override string PanelName => "Main menu";

        private Button _play, _inventory, _armory, _settings, _statistics, _saves, _commands, _info;

        public MainMenu(VisualTreeAsset asset,
                        PanelSettings settings,
                        UIServices services) : base(asset, settings, services) { }

        protected override void OnBind()
        {
            _play = Document.rootVisualElement.Q<VisualElement>("Play-button").Q<Button>();
            _play.RegisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _play.clicked += OnPlayButtonClicked;

            Services.GameControls.Navigation.Forward.performed += (_) => OnPlayButtonClicked();

            _inventory = Document.rootVisualElement.Q<VisualElement>("Inventory-button").Q<Button>();
            _inventory.RegisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _inventory.clicked += OnInventoryButtonClicked;

            Services.GameControls.Navigation.Inventory.performed += (_) => OnInventoryButtonClicked();

            _armory = Document.rootVisualElement.Q<VisualElement>("Armory-button").Q<Button>();
            _armory.RegisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _armory.clicked += OnArmoryButtonClicked;

            _settings = Document.rootVisualElement.Q<VisualElement>("Settings-button").Q<Button>();
            _settings.RegisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _settings.clicked += OnSettingsButtonClicked;

            Services.GameControls.Navigation.Settings.performed += (_) => OnSettingsButtonClicked();

            _statistics = Document.rootVisualElement.Q<VisualElement>("Statistics-button").Q<Button>();
            _statistics.RegisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _statistics.clicked += OnStatisticsButtonClicked;

            _saves = Document.rootVisualElement.Q<VisualElement>("Saves-button").Q<Button>();
            _saves.RegisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _saves.clicked += OnSavesButtonClicked;

            _commands = Document.rootVisualElement.Q<VisualElement>("Commands-button").Q<Button>();
            _commands.RegisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _commands.clicked += OnCommandsButtonClicked;

            Services.GameControls.Navigation.CommandConsole.performed += (_) => OnCommandsButtonClicked();

            _info = Document.rootVisualElement.Q<VisualElement>("Info-button").Q<Button>();
            _info.RegisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _info.clicked += OnInfoButtonClicked;
        }

        protected override void OnClear()
        {
            _play.UnregisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _play.clicked -= OnPlayButtonClicked;
            _play = null;

            Services.GameControls.Navigation.Forward.performed -= (_) => OnPlayButtonClicked();

            _inventory.UnregisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _inventory.clicked -= OnInventoryButtonClicked;
            _inventory = null;

            Services.GameControls.Navigation.Inventory.performed -= (_) => OnInventoryButtonClicked();

            _armory.UnregisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _armory.clicked -= OnArmoryButtonClicked;
            _armory = null;

            _settings.UnregisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _settings.clicked -= OnSettingsButtonClicked;
            _settings = null;

            Services.GameControls.Navigation.Settings.performed -= (_) => OnSettingsButtonClicked();

            _statistics.UnregisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _statistics.clicked -= OnStatisticsButtonClicked;
            _statistics = null;

            _saves.UnregisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _saves.clicked -= OnSavesButtonClicked;
            _saves = null;

            _commands.UnregisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _commands.clicked -= OnCommandsButtonClicked;
            _commands = null;

            Services.GameControls.Navigation.CommandConsole.performed -= (_) => OnCommandsButtonClicked();

            _info.UnregisterCallback<PointerOverEvent>(e => OnHoveredOverButton(e));
            _info.clicked -= OnInfoButtonClicked;
            _info = null;
        }

        protected override async UniTask LocalizeAsync()
        {
            var operation = LocalizationSettings.InitializationOperation;
            await UniTask.WaitUntil(() => operation.IsDone == true);

            Font font = await Services.Localizer.GetLocalizedRegularFontAsync();

            _play.style.unityFont = font;
            _play.text = await Services.Localizer.GetLocalizedStringAsync("Main menu", "Play");

            _inventory.style.unityFont = font;
            _inventory.text = await Services.Localizer.GetLocalizedStringAsync("Main menu", "Inventory");

            _armory.style.unityFont = font;
            _armory.text = await Services.Localizer.GetLocalizedStringAsync("Main menu", "Armory");

            _settings.style.unityFont = font;
            _settings.text = await Services.Localizer.GetLocalizedStringAsync("Main menu", "Settings");

            _statistics.style.unityFont = font;
            _statistics.text = await Services.Localizer.GetLocalizedStringAsync("Main menu", "Statistics");

            _saves.style.unityFont = font;
            _saves.text = await Services.Localizer.GetLocalizedStringAsync("Main menu", "Saves");

            _commands.style.unityFont = font;
            _commands.text = await Services.Localizer.GetLocalizedStringAsync("Main menu", "Commands");

            _info.style.unityFont = font;
            _info.text = await Services.Localizer.GetLocalizedStringAsync("Main menu", "Info");
        }

        #region buttons

        private void OnPlayButtonClicked()
        {
            if (IsLocked == true)
            {
                return;
            }

            PlayButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnInventoryButtonClicked()
        {
            if (IsLocked == true)
            {
                return;
            }

            InventoryButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnArmoryButtonClicked()
        {
            if (IsLocked == true)
            {
                return;
            }

            ArmoryButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnSettingsButtonClicked()
        {
            if (IsLocked == true)
            {
                return;
            }

            SettingsButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnStatisticsButtonClicked()
        {
            if (IsLocked == true)
            {
                return;
            }

            StatisticsButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnSavesButtonClicked()
        {
            if (IsLocked == true)
            {
                return;
            }

            SavesButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnCommandsButtonClicked()
        {
            if (IsLocked == true)
            {
                return;
            }

            CommandsButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnInfoButtonClicked()
        {
            if (IsLocked == true)
            {
                return;
            }

            InfoButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnHoveredOverButton(PointerOverEvent e)
        {
            if (IsLocked == true)
            {
                return;
            }

            HoveredOverButton?.Invoke(this, new(e));
        }

        #endregion
    }
}