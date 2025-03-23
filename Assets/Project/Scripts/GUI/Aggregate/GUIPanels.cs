using System;

using VContainer;

namespace SpaceAce.GUI
{
    public sealed record GUIPanels
    {
        public readonly MainMenu MainMenu;

        [Inject]
        public GUIPanels(MainMenu mainMenu)
        {
            MainMenu = mainMenu == null ? throw new ArgumentNullException() : mainMenu;
        }
    }
}