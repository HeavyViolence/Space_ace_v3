using System;

using UnityEngine;

namespace SpaceAce.Auxiliary.EventStreaming
{
    public interface IEvent
    {
        Guid SenderID { get; }
        DateTime Time { get; }
        Vector2 Position { get; }
    }
}