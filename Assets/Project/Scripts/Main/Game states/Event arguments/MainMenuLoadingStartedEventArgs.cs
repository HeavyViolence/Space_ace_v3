using System;

namespace SpaceAce.Main.GameStates
{
    public sealed class MainMenuLoadingStartedEventArgs : EventArgs
    {
        public float LoadDelay { get; }

        public MainMenuLoadingStartedEventArgs(float loadDelay)
        {
            if (loadDelay <= 0f)
            {
                throw new ArgumentOutOfRangeException();
            }

            LoadDelay = loadDelay;
        }
    }
}