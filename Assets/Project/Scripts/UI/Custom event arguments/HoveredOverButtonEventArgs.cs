using System;

using UnityEngine.UIElements;

namespace SpaceAce.UI
{
    public sealed class HoveredOverButtonEventArgs : EventArgs
    {
        public PointerOverEvent Data { get; }

        public HoveredOverButtonEventArgs(PointerOverEvent data)
        {
            Data = data;
        }
    }
}