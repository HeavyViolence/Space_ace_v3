using Cysharp.Threading.Tasks;

using SpaceAce.Gameplay.Levels;

using System;

namespace SpaceAce.UI
{
    public sealed class LevelSelectionMenuMediator : UIPanelMediator
    {
        protected override UIPanel Target => Panels.LevelSelectionMenu;

        public LevelSelectionMenuMediator(UIPanels panels, UIServices services)
            : base(panels, services) { }

        protected override void OnPanelEnabled()
        {
            Panels.LevelSelectionMenu.BattleButtonClicked += BattleButtoClickedEventHandler;
            Panels.LevelSelectionMenu.BackButtonCLicked += BackButtonClickedEventHandler;
            Panels.LevelSelectionMenu.SettingsButtonClicked += SettingsButtonClickedEventHandler;
            Panels.LevelSelectionMenu.InventoryButtonClicked += InventroyButtonClickedEventHandler;

            Panels.LevelSelectionMenu.LevelButtonClicked += LevelButtonClickedEventHandler;
            Panels.LevelSelectionMenu.LevelButtonReleased += LevelButtonReleasedEventHandler;

            Panels.LevelSelectionMenu.HoveredOverButton += HoveredOverButtonEventHandler;
        }

        protected override void OnPanelDisabled()
        {
            Panels.LevelSelectionMenu.BattleButtonClicked -= BattleButtoClickedEventHandler;
            Panels.LevelSelectionMenu.BackButtonCLicked -= BackButtonClickedEventHandler;
            Panels.LevelSelectionMenu.SettingsButtonClicked -= SettingsButtonClickedEventHandler;
            Panels.LevelSelectionMenu.InventoryButtonClicked -= InventroyButtonClickedEventHandler;

            Panels.LevelSelectionMenu.LevelButtonClicked -= LevelButtonClickedEventHandler;
            Panels.LevelSelectionMenu.LevelButtonReleased -= LevelButtonReleasedEventHandler;

            Panels.LevelSelectionMenu.HoveredOverButton -= HoveredOverButtonEventHandler;
        }

        #region buttons

        private void BattleButtoClickedEventHandler(object sender, EventArgs e)
        {
            OnButtonClicked();
        }

        private void BackButtonClickedEventHandler(object sender, EventArgs e)
        {
            Panels.LevelSelectionMenu.Disable();
            Panels.MainMenu.EnableAsync().Forget();

            OnButtonClicked();
        }

        private void SettingsButtonClickedEventHandler(object sender, EventArgs e)
        {
            OnButtonClicked();
        }

        private void InventroyButtonClickedEventHandler(object sender, EventArgs e)
        {
            OnButtonClicked();
        }

        private void LevelButtonClickedEventHandler(object sender, LevelButtonEventArgs e)
        {
            if (Services.BestLevelRunStatisticsCollector.TryGetStatistics(e.Level, out var statistics) == true)
            {
                LevelRewardBundle reward = Services.LevelRewardCollector.GetReward(e.Level);

                Panels.LevelSelectionMenu.DisplayLevelRewardsAsync(reward).Forget();
                Panels.LevelSelectionMenu.DisplayLevelMasteryAsync(statistics).Forget();
                Panels.LevelSelectionMenu.DisplayLevelStatisticsAsync(statistics).Forget();
            }
            else
            {
                Panels.LevelSelectionMenu.ClearLevelRewardsDisplayAsync().Forget();
                Panels.LevelSelectionMenu.ClearLevelMasteryDisplayAsync().Forget();
                Panels.LevelSelectionMenu.ClearLevelStatisticsDisplayAsync().Forget();
            }

            OnButtonClicked();
        }

        private void LevelButtonReleasedEventHandler(object sender, LevelButtonEventArgs e)
        {
            Panels.LevelSelectionMenu.ClearLevelRewardsDisplayAsync().Forget();
            Panels.LevelSelectionMenu.ClearLevelMasteryDisplayAsync().Forget();
            Panels.LevelSelectionMenu.ClearLevelStatisticsDisplayAsync().Forget();
        }

        private void HoveredOverButtonEventHandler(object sender, HoveredOverButtonEventArgs e)
        {
            OnHoveredOver();
        }

        #endregion
    }
}