using Moonlight.Clients;
using Moonlight.Game.Entities;

namespace Moonlight.Event.Entities
{
    public class EntityDeathEvent : IEventNotification
    {
        public EntityDeathEvent(Client emitter) => Emitter = emitter;

        public LivingEntity Entity { get; set; }
        public LivingEntity Killer { get; set; }
        public Client Emitter { get; }
    }
}