using Moonlight.Clients;
using Moonlight.Game.Entities;

namespace Moonlight.Event.Characters
{
    public class CharacterInitializeEvent : IEventNotification
    {
        public CharacterInitializeEvent(Client emitter) => Emitter = emitter;
        public Character Character { get; internal set; }
        public Client Emitter { get; }
    }
}