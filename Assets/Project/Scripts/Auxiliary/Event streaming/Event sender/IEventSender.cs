using System;

namespace SpaceAce.Auxiliary.EventStreaming
{
    public interface IEventSender<T> where T : IEvent
    {
        Guid ID { get; }
        void Bind(IEventLink<T> link);
    }
}