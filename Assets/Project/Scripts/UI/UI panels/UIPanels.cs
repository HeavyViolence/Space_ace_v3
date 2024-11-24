using System;

namespace SpaceAce.UI
{
    public sealed record UIPanels
    {
        public MainMenu MainMenu { get; }

        public UIPanels(MainMenu mainMenu)
        {
            MainMenu = mainMenu ?? throw new ArgumentNullException();
        }
    }
}