using SpaceAce.Auxiliary.EventArguments;

using System;

namespace SpaceAce.Main.Saving
{
    public interface ISavable : IEquatable<ISavable>
    {
        event EventHandler SavingRequested;
        event EventHandler<ErrorOccurredEventArgs> ErrorOccurred;

        string SavedDataName { get; }

        string GetState();

        void SetState(string state);
        void SetDefaultState();
    }
}