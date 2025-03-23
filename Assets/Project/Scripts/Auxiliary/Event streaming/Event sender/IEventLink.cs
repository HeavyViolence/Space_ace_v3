using System;

namespace SpaceAce.Auxiliary.EventStreaming
{
    public interface IEventLink<T> where T : IEvent
    {
        void Send();
        void Send(T obj);
        void ErrorOut(Exception ex);
        void Complete();
    }
}