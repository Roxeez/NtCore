using Moonlight.Clients;

namespace Moonlight.Event.Maps
{
    public class MapCleanEvent : IEventNotification
    {
        public MapCleanEvent(Client emitter) => Emitter = emitter;

        public Client Emitter { get; }
    }
}