using Cysharp.Threading.Tasks;

using SpaceAce.Main;

using System;

namespace SpaceAce.GUI
{
    public sealed class MainMenuMediator : GUIPanelMediator
    {
        public MainMenuMediator(GUIPanels panels, MainServices services) :
            base(panels.MainMenu, panels, services)
        { }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            EnablePanelAsync().Forget();

            Panels.MainMenu.PlayButtonClicked += PlayButtonClickedEventHandler;
            Panels.MainMenu.InventoryButtonClicked += InventoryButtonClickedEventHandler;
            Panels.MainMenu.ArmoryButtonClicked += ArmoryButtonClickedEventHandler;
            Panels.MainMenu.SettingsButtonClicked += SettingsButtonClickedEventHandler;
            Panels.MainMenu.StatisticsButtonClicked += StatisticsButtonClickedEventHandler;
            Panels.MainMenu.AuthorizationButtonClicked += AuthorizationButtonClickedEventHandler;
            Panels.MainMenu.CheatsButtonClicked += CheatsButtonClickedEventHandler;
            Panels.MainMenu.HoveredOverButton += OnHoveredOverButton;
        }

        protected override void OnDispose()
        {
            base.OnDispose();

            Panels.MainMenu.PlayButtonClicked -= PlayButtonClickedEventHandler;
            Panels.MainMenu.InventoryButtonClicked -= InventoryButtonClickedEventHandler;
            Panels.MainMenu.ArmoryButtonClicked -= ArmoryButtonClickedEventHandler;
            Panels.MainMenu.SettingsButtonClicked -= SettingsButtonClickedEventHandler;
            Panels.MainMenu.StatisticsButtonClicked -= StatisticsButtonClickedEventHandler;
            Panels.MainMenu.AuthorizationButtonClicked -= AuthorizationButtonClickedEventHandler;
            Panels.MainMenu.CheatsButtonClicked -= CheatsButtonClickedEventHandler;
            Panels.MainMenu.HoveredOverButton -= OnHoveredOverButton;
        }

        #region event handlers

        private void PlayButtonClickedEventHandler(object sender, EventArgs e)
        {
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

        private void AuthorizationButtonClickedEventHandler(object sender, EventArgs e)
        {
            OnButtonClicked();
        }

        private void CheatsButtonClickedEventHandler(object sender, EventArgs e)
        {
            OnButtonClicked();
        }

        #endregion
    }
}