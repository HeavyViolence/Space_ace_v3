using System;

namespace SpaceAce.Auxiliary.EventStreaming
{
    public interface IEventReceiver<T> where T : IEvent
    {
        Guid ID { get; }

        void OnEvent();
        void OnEvent(T obj);
        void OnError(Exception ex);
        void OnCompleted();
    }
}
