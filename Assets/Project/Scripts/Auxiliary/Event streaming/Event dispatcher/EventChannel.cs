using System;
using System.Collections.Generic;

namespace SpaceAce.Auxiliary.EventStreaming
{
    public sealed class EventChannel<T> : IEventLink<T>, IEventRelay<T>, IDisposable where T : IEvent
    {
        private readonly HashSet<IEventReceiver<T>> _receivers = new(new EventReceiverComparer<T>());

        public void Add(IEventReceiver<T> receiver) => _receivers.Add(receiver);
        public void Remove(IEventReceiver<T> receiver) => _receivers.Remove(receiver);

        public void Send()
        {
            if (_receivers.Count == 0)
            {
                return;
            }

            foreach (IEventReceiver<T> receiver in _receivers)
            {
                receiver.OnEvent();
            }
        }

        public void Send(T obj)
        {
            if (_receivers.Count == 0)
            {
                return;
            }

            foreach (IEventReceiver<T> receiver in _receivers)
            {
                receiver.OnEvent(obj);
            }
        }

        public void ErrorOut(Exception ex)
        {
            if (_receivers.Count == 0)
            {
                return;
            }

            foreach (IEventReceiver<T> receiver in _receivers)
            {
                receiver.OnError(ex);
            }
        }

        public void Complete()
        {
            if (_receivers.Count == 0)
            {
                return;
            }

            foreach (IEventReceiver<T> receiver in _receivers)
            {
                receiver.OnCompleted();
            }
        }

        public void Dispose()
        {
            if (_receivers.Count == 0)
            {
                return;
            }

            _receivers.Clear();
        }
    }
}