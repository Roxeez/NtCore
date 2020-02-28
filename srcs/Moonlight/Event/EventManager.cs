using System;
using System.Collections.Generic;
using Moonlight.Extensions;

namespace Moonlight.Event
{
    internal class EventManager : IEventManager
    {
        private readonly Dictionary<Type, List<IEventListener>> _handlers;

        public EventManager() => _handlers = new Dictionary<Type, List<IEventListener>>();

        public void Emit<T>(T notification) where T : IEventNotification
        {
            List<IEventListener> handlers = _handlers.GetValueOrDefault(typeof(T));
            if (handlers == null)
            {
                return;
            }

            handlers.ForEach(x => x.Handle(notification));
        }

        public void RegisterListener<T>(EventListener<T> listener) where T : IEventNotification
        {
            Type type = typeof(T);
            List<IEventListener> handlers = _handlers.GetValueOrDefault(type);
            if (handlers == null)
            {
                handlers = new List<IEventListener>();
                _handlers[type] = handlers;
            }

            handlers.Add(listener);
        }
    }
}