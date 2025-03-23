using System;

namespace SpaceAce.Main.Saving
{
    public interface ISavable
    {
        event Action StateChanged;

        string StateName { get; }

        byte[] GetState();
        void SetState(byte[] state);
    }
}