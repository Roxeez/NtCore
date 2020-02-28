using Moonlight.Clients;
using Moonlight.Game.Entities;
using Moonlight.Game.Maps;

namespace Moonlight.Event.Maps
{
    public class EntityJoinEvent : IEventNotification
    {
        public EntityJoinEvent(Client emitter) => Emitter = emitter;

        public Map Map { get; internal set; }
        public Entity Entity { get; internal set; }
        public Client Emitter { get; }
    }
}