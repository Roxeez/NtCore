using Moonlight.Clients;
using Moonlight.Game.Battle;
using Moonlight.Game.Entities;

namespace Moonlight.Event.Entities
{
    public class EntityDamageEvent : IEventNotification
    {
        public EntityDamageEvent(Client emitter) => Emitter = emitter;

        public LivingEntity Entity { get; internal set; }
        public LivingEntity Attacker { get; internal set; }
        public int Damage { get; internal set; }
        public Skill Skill { get; internal set; }
        public Client Emitter { get; }
    }
}