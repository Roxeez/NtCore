using Moonlight.Clients;
using Moonlight.Game.Entities;

namespace Moonlight.Event.Characters
{
    public class StatChangeEvent : IEventNotification
    {
        public StatChangeEvent(Client emitter) => Emitter = emitter;

        public Character Character { get; set; }
        public Client Emitter { get; }
    }
}