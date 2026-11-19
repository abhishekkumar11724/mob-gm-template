using System;

namespace Nimbus.Core;

public static class EventBus
{
    public static void Subscribe<T>(Action<T> handler)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        HandlerStorage<T>.Subscribe(handler);
    }

    public static void Unsubscribe<T>(Action<T> handler)
    {
        if (handler == null)
        {
            throw new ArgumentNullException(nameof(handler));
        }

        HandlerStorage<T>.Unsubscribe(handler);
    }

    public static void Publish<T>(T eventData)
    {
        if (eventData == null)
        {
            throw new ArgumentNullException(nameof(eventData));
        }

        HandlerStorage<T>.Publish(eventData);
    }

    private static class HandlerStorage<T>
    {
        private static Action<T>? _onEvent;

        public static void Subscribe(Action<T> handler)
        {
            _onEvent += handler;
        }

        public static void Unsubscribe(Action<T> handler)
        {
            _onEvent -= handler;
        }

        public static void Publish(T eventData)
        {
            _onEvent?.Invoke(eventData);
        }
    }
}
