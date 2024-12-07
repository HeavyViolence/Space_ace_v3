using Cysharp.Threading.Tasks;

using SpaceAce.Gameplay.Levels;

using System;
using System.Collections.Generic;

using UnityEngine.UIElements;

namespace SpaceAce.UI
{
    public sealed class LevelSelectionMenu : UIPanel
    {
        private const float BattleButtonLockDelay = 0.1f;

        public event EventHandler BattleButtonClicked,
                                  BackButtonCLicked,
                                  SettingsButtonClicked,
                                  InventoryButtonClicked;

        public event EventHandler<LevelButtonEventArgs> LevelButtonClicked,
                                                        LevelButtonReleased;

        public event EventHandler<HoveredOverButtonEventArgs> HoveredOverButton;

        private readonly LevelSelectionMenuConfig _config;

        protected override string PanelName => "Level selection menu";

        public LevelSelectionMenu(VisualTreeAsset asset,
                                  PanelSettings settings,
                                  UIServices services,
                                  LevelSelectionMenuConfig config) : base(asset, settings, services)
        {
            _config = config == null ? throw new ArgumentNullException() : config;
        }

        #region bindings

        private Label _menuHeader;
        private VisualElement _levelButtonsAnchor;

        private Button _battle, _back;
        private List<Button> _levelButtons;

        private Label _levelCompletionRewardHeader,
                      _levelMasteryRewardHeader,
                      _levelExcellenceRewardHeader;

        private Label _levelCompletionReward,
                      _levelMasteryReward,
                      _levelExcellenceReward;

        private Label _levelMasteryHeader,
                      _damageMasteryHeader,
                      _accuracyMasteryHeader,
                      _dodgingMasteryHeader,
                      _meteorMasteryHeader,
                      _wreckMasteryHeader,
                      _experienceMasteryHeader,
                      _masteryTotalHeader;

        private VisualElement _damageMasteryBar,
                              _accuracyMasteryBar,
                              _dodgingMasteryBar,
                              _meteorMasteryBar,
                              _wreckMasteryBar,
                              _experienceMasteryBar,
                              _masteryTotalBar;

        private Label _levelStatisticsHeader,
                      _levelStatisticsContent;

        protected override void OnBind()
        {
            _menuHeader = Document.rootVisualElement.Q<VisualElement>("Menu-nameplate").Q<Label>("Menu-name-label");

            BindButtons();
            BindLevelRewardPanels();
            BindLevelMasteryPanel();
            BindLevelStatisticsPanel();
            AddUnlockedLevelsButtons();
        }

        protected override void OnClear()
        {
            _menuHeader = null;

            ClearButtons();
            ClearLevelRewardPanels();
            ClearLevelMasteryPanel();
            ClearLevelStatisticsPanel();
            ClearUnlockedLevelsButtons();
        }

        private void BindButtons()
        {
            _battle = Document.rootVisualElement.Q<VisualElement>("Battle-button").Q<Button>("Button");
            _battle.SetEnabled(false);
            _battle.clicked += OnBattleButtonClicked;

            _back = Document.rootVisualElement.Q<VisualElement>("Back-button").Q<Button>("Button");
            _back.clicked += OnBackButtonClicked;
            _back.RegisterCallback<PointerOverEvent>((e) => OnHoveredOverButton(e));

            Services.GameControls.Navigation.Forward.performed += (_) => OnBattleButtonClicked();
            Services.GameControls.Navigation.Back.performed += (_) => OnBackButtonClicked();
            Services.GameControls.Navigation.Settings.performed += (_) => OnSettingsButtonClicked();
            Services.GameControls.Navigation.Inventory.performed += (_) => OnInventoryButtonClicked();
        }

        private void ClearButtons()
        {
            _battle.clicked -= OnBattleButtonClicked;
            _battle.UnregisterCallback<PointerOverEvent>((e) => OnHoveredOverButton(e));
            _battle = null;

            _back.clicked -= OnBackButtonClicked;
            _back.UnregisterCallback<PointerOverEvent>((e) => OnHoveredOverButton(e));
            _back = null;

            Services.GameControls.Navigation.Forward.performed -= (_) => OnBattleButtonClicked();
            Services.GameControls.Navigation.Back.performed -= (_) => OnBackButtonClicked();
            Services.GameControls.Navigation.Settings.performed -= (_) => OnSettingsButtonClicked();
            Services.GameControls.Navigation.Inventory.performed -= (_) => OnInventoryButtonClicked();
        }

        private void BindLevelRewardPanels()
        {
            var levelCompletionRewardPanel = Document.rootVisualElement.Q<VisualElement>("Level-completion-reward-panel");
            _levelCompletionRewardHeader = levelCompletionRewardPanel.Q<Label>("Level-reward-header-label");
            _levelCompletionReward = levelCompletionRewardPanel.Q<Label>("Level-reward-label");

            var levelMasteryRewardPanel = Document.rootVisualElement.Q<VisualElement>("Level-mastery-reward-panel");
            _levelMasteryRewardHeader = levelMasteryRewardPanel.Q<Label>("Level-reward-header-label");
            _levelMasteryReward = levelMasteryRewardPanel.Q<Label>("Level-reward-label");

            var levelExcellenceRewardPanel = Document.rootVisualElement.Q<VisualElement>("Level-excellence-reward-panel");
            _levelExcellenceRewardHeader = levelExcellenceRewardPanel.Q<Label>("Level-reward-header-label");
            _levelExcellenceReward = levelExcellenceRewardPanel.Q<Label>("Level-reward-label");
        }

        private void ClearLevelRewardPanels()
        {
            _levelCompletionRewardHeader = null;
            _levelCompletionReward = null;

            _levelMasteryRewardHeader = null;
            _levelMasteryReward = null;

            _levelExcellenceRewardHeader = null;
            _levelExcellenceReward = null;
        }

        private void BindLevelStatisticsPanel()
        {
            var levelStatisticsPanel = Document.rootVisualElement.Q<VisualElement>("Level-statistics-panel");
            _levelStatisticsHeader = levelStatisticsPanel.Q<Label>("Header-label");
            _levelStatisticsContent = levelStatisticsPanel.Q<Label>("Content-label");
        }

        private void ClearLevelStatisticsPanel()
        {
            _levelStatisticsHeader = null;
            _levelStatisticsContent = null;
        }

        private void BindLevelMasteryPanel()
        {
            _levelMasteryHeader = Document.rootVisualElement.Q<VisualElement>("Level-mastery-panel")
                                                            .Q<Label>("Header-label");

            var damagePanel = Document.rootVisualElement.Q<VisualElement>("Damage-panel");
            _damageMasteryHeader = damagePanel.Q<Label>("Progress-label");
            _damageMasteryBar = damagePanel.Q<VisualElement>("Progress-bar-foreground");

            var accuracyPanel = Document.rootVisualElement.Q<VisualElement>("Accuracy-panel");
            _accuracyMasteryHeader = accuracyPanel.Q<Label>("Progress-label");
            _accuracyMasteryBar = accuracyPanel.Q<VisualElement>("Progress-bar-foreground");

            var dodgingPanel = Document.rootVisualElement.Q<VisualElement>("Dodging-panel");
            _dodgingMasteryHeader = dodgingPanel.Q<Label>("Progress-label");
            _dodgingMasteryBar = dodgingPanel.Q<VisualElement>("Progress-bar-foreground");

            var meteorsDestroyedPanel = Document.rootVisualElement.Q<VisualElement>("Meteors-destroyed-panel");
            _meteorMasteryHeader = meteorsDestroyedPanel.Q<Label>("Progress-label");
            _meteorMasteryBar = meteorsDestroyedPanel.Q<VisualElement>("Progress-bar-foreground");

            var wrecksDestroyedPanel = Document.rootVisualElement.Q<VisualElement>("Wrecks-destroyed-panel");
            _wreckMasteryHeader = wrecksDestroyedPanel.Q<Label>("Progress-label");
            _wreckMasteryBar = wrecksDestroyedPanel.Q<VisualElement>("Progress-bar-foreground");

            var experiencePanel = Document.rootVisualElement.Q<VisualElement>("Experience-panel");
            _experienceMasteryHeader = experiencePanel.Q<Label>("Progress-label");
            _experienceMasteryBar = experiencePanel.Q<VisualElement>("Progress-bar-foreground");

            var masteryTotalPanel = Document.rootVisualElement.Q<VisualElement>("Mastery-total-panel");
            _masteryTotalHeader = masteryTotalPanel.Q<Label>("Progress-label");
            _masteryTotalBar = masteryTotalPanel.Q<VisualElement>("Progress-bar-foreground");
        }

        private void ClearLevelMasteryPanel()
        {
            _damageMasteryHeader = null;
            _damageMasteryBar = null;

            _accuracyMasteryHeader = null;
            _accuracyMasteryBar = null;
            
            _dodgingMasteryHeader = null;
            _dodgingMasteryBar = null;

            _meteorMasteryHeader = null;
            _meteorMasteryBar = null;

            _wreckMasteryHeader = null;
            _wreckMasteryBar = null;

            _experienceMasteryHeader = null;
            _experienceMasteryBar = null;

            _masteryTotalHeader = null;
            _masteryTotalBar = null;
        }

        private void AddUnlockedLevelsButtons()
        {
            _levelButtonsAnchor = Document.rootVisualElement.Q<VisualElement>("Level-buttons-anchor");

            int levelsUnlocked = Services.LevelUnlocker.LastUnlockedLevel;
            _levelButtons = new(levelsUnlocked);

            for (int i = 1; i <= levelsUnlocked; i++)
            {
                var levelButton = _config.LevelButton;
                levelButton.name = $"Level-{i}-button";

                Button button = levelButton.Q<Button>("Button");
                button.text = i.ToString();
                button.clicked += () => OnLevelButtonClicked(i);
                button.RegisterCallback<PointerOverEvent>((e) => OnHoveredOverButton(e));
                button.RegisterCallback<FocusOutEvent>(async (_) => await OnLevelButtonReleasedAsync(i));

                _levelButtonsAnchor.Add(levelButton);
                _levelButtons.Add(button);
            }
        }

        private void ClearUnlockedLevelsButtons()
        {
            for (int i = 0; i < _levelButtons.Count; i++)
            {
                Button button = _levelButtons[i];

                button.clicked -= () => OnLevelButtonClicked(i + 1);
                button.UnregisterCallback<PointerOverEvent>((e) => OnHoveredOverButton(e));
                button.UnregisterCallback<FocusOutEvent>(async (_) => await OnLevelButtonReleasedAsync(i + 1));
            }

            _levelButtonsAnchor = null;
            _levelButtons.Clear();
        }

        #endregion

        #region localization

        protected async override UniTask LocalizeAsync()
        {
            _menuHeader.text = await Services.Localizer.GetLocalizedStringAsync("Level selection menu", "Menu header");

            await LocalizeButtonsAsync();
            await LocalizeLevelRewardsAsync();
            await LocalizeLevelMasteryAsync();
            await LocalizeLevelStatisticsAsync();
        }

        private async UniTask LocalizeButtonsAsync()
        {
            _battle.text = await Services.Localizer.GetLocalizedStringAsync("Level selection menu", "Battle");
            _back.text = await Services.Localizer.GetLocalizedStringAsync("Level selection menu", "Back");
        }

        private async UniTask LocalizeLevelRewardsAsync()
        {
            _levelCompletionRewardHeader.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level completion reward");

            _levelMasteryRewardHeader.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level mastery reward");

            _levelExcellenceRewardHeader.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level excellence reward");

            _levelCompletionReward.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level credits and experience reward", LevelReward.Empty);

            _levelMasteryReward.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level credits and experience reward", LevelReward.Empty);

            _levelExcellenceReward.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level credits and experience reward", LevelReward.Empty);
        }

        private async UniTask LocalizeLevelMasteryAsync()
        {
            _damageMasteryHeader.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Damage mastery", LevelStatistics.Default.DamageMasteryPercent);

            _damageMasteryBar.style.width = Length.Percent(LevelStatistics.Default.DamageMasteryPercent);

            _accuracyMasteryHeader.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Accuracy", LevelStatistics.Default.AccuracyPercent);

            _accuracyMasteryBar.style.width = Length.Percent(LevelStatistics.Default.AccuracyPercent);

            _dodgingMasteryHeader.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Dodging mastery", LevelStatistics.Default.DodgingMasteryPercent);

            _dodgingMasteryBar.style.width = Length.Percent(LevelStatistics.Default.DodgingMasteryPercent);

            _meteorMasteryHeader.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Meteor mastery", LevelStatistics.Default.MeteorMasteryPercent);

            _meteorMasteryBar.style.width = Length.Percent(LevelStatistics.Default.MeteorMasteryPercent);

            _wreckMasteryHeader.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Wreck mastery", LevelStatistics.Default.WreckMasteryPercent);

            _wreckMasteryBar.style.width = Length.Percent(LevelStatistics.Default.WreckMasteryPercent);

            _experienceMasteryHeader.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Experience mastery", LevelStatistics.Default.ExperienceMasteryPercent);

            _experienceMasteryBar.style.width = Length.Percent(LevelStatistics.Default.ExperienceMasteryPercent);

            _masteryTotalHeader.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Mastery total", LevelStatistics.Default.MasteryTotalPercent);

            _masteryTotalBar.style.width = Length.Percent(LevelStatistics.Default.MasteryTotalPercent);
        }

        private async UniTask LocalizeLevelStatisticsAsync()
        {
            _levelStatisticsHeader.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level statistics header");

            _levelStatisticsContent.text = await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Empty level statistics");
        }

        #endregion

        #region API

        public async UniTask DisplayLevelRewardsAsync(LevelRewardBundle reward)
        {
            if (Active == false || IsLocked == true)
            {
                return;
            }

            if (reward is null)
            {
                throw new ArgumentNullException();
            }

            _levelCompletionReward.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level credits and experience reward", reward.CompletionReward);

            _levelMasteryReward.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level credits and experience reward", reward.MasteryReward);

            _levelExcellenceReward.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level credits and experience reward", reward.ExcellenceReward);
        }

        public async UniTask ClearLevelRewardsDisplayAsync()
        {
            if (Active == false || IsLocked == true)
            {
                return;
            }

            _levelCompletionReward.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level credits and experience reward", LevelReward.Empty);

            _levelMasteryReward.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level credits and experience reward", LevelReward.Empty);

            _levelExcellenceReward.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level credits and experience reward", LevelReward.Empty);
        }

        public async UniTask DisplayLevelMasteryAsync(LevelStatistics statistics)
        {
            if (Active == false || IsLocked == true)
            {
                return;
            }

            if (statistics is null)
            {
                throw new ArgumentNullException();
            }

            _damageMasteryHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Damage mastery", statistics.DamageMasteryPercent);

            _damageMasteryBar.style.width = Length.Percent(statistics.DamageMasteryPercent);

            _accuracyMasteryHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Accuracy", statistics.AccuracyPercent);

            _accuracyMasteryBar.style.width = Length.Percent(statistics.AccuracyPercent);

            _dodgingMasteryHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Dodging mastery", statistics.DodgingMasteryPercent);

            _dodgingMasteryBar.style.width = Length.Percent(statistics.DodgingMasteryPercent);

            _meteorMasteryHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Meteor mastery", statistics.MeteorMasteryPercent);

            _meteorMasteryBar.style.width = Length.Percent(statistics.MeteorMasteryPercent);

            _wreckMasteryHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Wreck mastery", statistics.WreckMasteryPercent);

            _wreckMasteryBar.style.width = Length.Percent(statistics.WreckMasteryPercent);

            _experienceMasteryHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Experience mastery", statistics.ExperienceMasteryPercent);

            _experienceMasteryBar.style.width = Length.Percent(statistics.ExperienceMasteryPercent);

            _masteryTotalHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Mastery total", statistics.MasteryTotalPercent);

            _masteryTotalBar.style.width = Length.Percent(statistics.MasteryTotalPercent);
        }

        public async UniTask ClearLevelMasteryDisplayAsync()
        {
            if (Active == false || IsLocked == true)
            {
                return;
            }

            _damageMasteryHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Damage mastery", LevelStatistics.Default.DamageMasteryPercent);

            _damageMasteryBar.style.width = Length.Percent(LevelStatistics.Default.DamageMasteryPercent);

            _accuracyMasteryHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Accuracy", LevelStatistics.Default.AccuracyPercent);

            _accuracyMasteryBar.style.width = Length.Percent(LevelStatistics.Default.AccuracyPercent);

            _dodgingMasteryHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Dodging mastery", LevelStatistics.Default.DodgingMasteryPercent);

            _dodgingMasteryBar.style.width = Length.Percent(LevelStatistics.Default.DodgingMasteryPercent);

            _meteorMasteryHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Meteor mastery", LevelStatistics.Default.MeteorMasteryPercent);

            _meteorMasteryBar.style.width = Length.Percent(LevelStatistics.Default.MeteorMasteryPercent);

            _wreckMasteryHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Wreck mastery", LevelStatistics.Default.WreckMasteryPercent);

            _wreckMasteryBar.style.width = Length.Percent(LevelStatistics.Default.WreckMasteryPercent);

            _experienceMasteryHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Experience mastery", LevelStatistics.Default.ExperienceMasteryPercent);

            _experienceMasteryBar.style.width = Length.Percent(LevelStatistics.Default.ExperienceMasteryPercent);

            _masteryTotalHeader.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Mastery total", LevelStatistics.Default.MasteryTotalPercent);

            _masteryTotalBar.style.width = Length.Percent(LevelStatistics.Default.MasteryTotalPercent);
        }

        public async UniTask DisplayLevelStatisticsAsync(LevelStatistics statistics)
        {
            if (Active == false || IsLocked == true)
            {
                return;
            }

            if (statistics is null)
            {
                throw new ArgumentNullException();
            }

            _levelStatisticsContent.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Level statistics", statistics);
        }

        public async UniTask ClearLevelStatisticsDisplayAsync()
        {
            if (Active == false || IsLocked == true)
            {
                return;
            }

            _levelStatisticsContent.text ??= await Services.Localizer.GetLocalizedStringAsync(
                "Level selection menu", "Empty level statistics");
        }

        #endregion

        #region event handlers

        private void OnLevelButtonClicked(int level)
        {
            if (IsLocked == true)
            {
                return;
            }

            _battle?.SetEnabled(true);
            LevelButtonClicked?.Invoke(this, new(level));
        }

        private async UniTask OnLevelButtonReleasedAsync(int level)
        {
            if (IsLocked == true)
            {
                return;
            }

            await UniTask.WaitForSeconds(BattleButtonLockDelay);

            _battle?.SetEnabled(false);
            LevelButtonReleased?.Invoke(this, new(level));
        }

        private void OnBattleButtonClicked()
        {
            if (IsLocked == true)
            {
                return;
            }

            BattleButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnBackButtonClicked()
        {
            if (IsLocked == true)
            {
                return;
            }

            BackButtonCLicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnSettingsButtonClicked()
        {
            if (IsLocked == true)
            {
                return;
            }

            SettingsButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnInventoryButtonClicked()
        {
            if (IsLocked == true)
            {
                return;
            }

            InventoryButtonClicked?.Invoke(this, EventArgs.Empty);
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