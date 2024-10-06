using System;

namespace SpaceAce.Main.GamePause
{
    public interface IPausable : IEquatable<IPausable>
    {
        Guid ID { get; }

        void Pause();
        void Resume();
    }
}