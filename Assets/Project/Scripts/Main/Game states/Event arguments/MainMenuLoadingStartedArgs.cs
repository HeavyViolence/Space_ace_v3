using System;

namespace SpaceAce.Main.GameStates
{
    public readonly struct MainMenuLoadingStartedArgs
    {
        public float LoadDelay { get; }

        public MainMenuLoadingStartedArgs(float loadDelay)
        {
            if (loadDelay <= 0f)
            {
                throw new ArgumentOutOfRangeException();
            }

            LoadDelay = loadDelay;
        }
    }
}