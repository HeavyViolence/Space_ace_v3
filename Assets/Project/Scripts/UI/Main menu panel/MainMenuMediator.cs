using Cysharp.Threading.Tasks;

using System;

namespace SpaceAce.UI
{
    public sealed class MainMenuMediator : UIPanelMediator
    {
        public MainMenuMediator(UIPanels panels, UIServices services) :
            base(panels, services) { }

        protected override UIPanel Target => Panels.MainMenu;

        protected override void OnInitialize()
        {
            Panels.MainMenu.EnableAsync().Forget();
        }

        protected override void OnPanelEnabled()
        {
            Panels.MainMenu.PlayButtonClicked += PlayButtonClickedEventHandler;
            Panels.MainMenu.InventoryButtonClicked += InventoryButtonClickedEventHandler;
            Panels.MainMenu.ArmoryButtonClicked += ArmoryButtonClickedEventHandler;
            Panels.MainMenu.SettingsButtonClicked += SettingsButtonClickedEventHandler;
            Panels.MainMenu.StatisticsButtonClicked += StatisticsButtonClickedEventHandler;
            Panels.MainMenu.SavesButtonClicked += SavesButtonClickedEventHandler;
            Panels.MainMenu.CommandsButtonClicked += CommandsButtonClickedEventHandler;
            Panels.MainMenu.InfoButtonClicked += InfoButtonClickedEventHandler;
            Panels.MainMenu.HoveredOverButton += HoveredOverButtonEventHandler;
        }

        protected override void OnPanelDisabled()
        {
            Panels.MainMenu.PlayButtonClicked -= PlayButtonClickedEventHandler;
            Panels.MainMenu.InventoryButtonClicked -= InventoryButtonClickedEventHandler;
            Panels.MainMenu.ArmoryButtonClicked -= ArmoryButtonClickedEventHandler;
            Panels.MainMenu.SettingsButtonClicked -= SettingsButtonClickedEventHandler;
            Panels.MainMenu.StatisticsButtonClicked -= StatisticsButtonClickedEventHandler;
            Panels.MainMenu.SavesButtonClicked -= SavesButtonClickedEventHandler;
            Panels.MainMenu.CommandsButtonClicked -= CommandsButtonClickedEventHandler;
            Panels.MainMenu.InfoButtonClicked -= InfoButtonClickedEventHandler;
            Panels.MainMenu.HoveredOverButton -= HoveredOverButtonEventHandler;
        }

        #region event handlers

        private void PlayButtonClickedEventHandler(object sender, EventArgs e)
        {
            Panels.MainMenu.Disable();
            Panels.LevelSelectionMenu.EnableAsync().Forget();

            OnButtonClicked();
        }

        private void InventoryButtonClickedEventHandler(object sender, EventArgs e)
        {
            OnButtonClicked();
        }

        private void ArmoryButtonClickedEventHandler(object sender, EventArgs e)
        {
            OnButtonClicked();
        }

        private void SettingsButtonClickedEventHandler(object sender, EventArgs e)
        {
            OnButtonClicked();
        }

        private void StatisticsButtonClickedEventHandler(object sender, EventArgs e)
        {
            OnButtonClicked();
        }

        private void SavesButtonClickedEventHandler(object sender, EventArgs e)
        {
            OnButtonClicked();
        }

        private void CommandsButtonClickedEventHandler(object sender, EventArgs e)
        {
            OnButtonClicked();
        }

        private void InfoButtonClickedEventHandler(object sender, EventArgs e)
        {
            OnButtonClicked();
        }

        private void HoveredOverButtonEventHandler(object sender, HoveredOverButtonEventArgs e)
        {
            OnHoveredOver();
        }

        #endregion
    }
}