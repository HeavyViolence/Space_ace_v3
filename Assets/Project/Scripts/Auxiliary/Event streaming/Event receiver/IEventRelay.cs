namespace SpaceAce.Auxiliary.EventStreaming
{
    public interface IEventRelay<T> where T : IEvent
    {
        void Add(IEventReceiver<T> receiver);
        void Remove(IEventReceiver<T> receiver);
    }
}