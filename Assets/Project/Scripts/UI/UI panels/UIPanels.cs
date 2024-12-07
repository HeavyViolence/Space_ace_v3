using System;

namespace SpaceAce.UI
{
    public sealed record UIPanels
    {
        public MainMenu MainMenu { get; }
        public LevelSelectionMenu LevelSelectionMenu { get; }

        public UIPanels(MainMenu mainMenu,
                        LevelSelectionMenu levelSelectionMenu)
        {
            MainMenu = mainMenu ?? throw new ArgumentNullException();
            LevelSelectionMenu = levelSelectionMenu ?? throw new ArgumentNullException();
        }
    }
}