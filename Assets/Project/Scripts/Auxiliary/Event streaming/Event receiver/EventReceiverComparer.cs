using System;
using System.Collections.Generic;

namespace SpaceAce.Auxiliary.EventStreaming
{
    public sealed class EventReceiverComparer<T> :
        IEqualityComparer<IEventReceiver<T>> where T : IEvent
    {
        public bool Equals(IEventReceiver<T> x, IEventReceiver<T> y)
        {
            if (x is null || y is null)
            {
                return false;
            }

            return x.ID == y.ID;
        }

        public int GetHashCode(IEventReceiver<T> obj) =>
            HashCode.Combine(obj.ID);
    }
}