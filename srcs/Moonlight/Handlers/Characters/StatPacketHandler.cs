using Moonlight.Clients;
using Moonlight.Event;
using Moonlight.Event.Characters;
using Moonlight.Game.Entities;
using Moonlight.Packet.Character;

namespace Moonlight.Handlers.Characters
{
    internal class StatPacketHandler : PacketHandler<StatPacket>
    {
        private readonly IEventManager _eventManager;

        public StatPacketHandler(IEventManager eventManager) => _eventManager = eventManager;

        protected override void Handle(Client client, StatPacket packet)
        {
            Character character = client.Character;

            if (character != null)
            {
                character.Hp = packet.Hp;
                character.Mp = packet.Mp;
                character.MaxHp = packet.MaxHp;
                character.MaxMp = packet.MaxMp;

                _eventManager.Emit(new StatChangeEvent(client)
                {
                    Character = client.Character
                });
            }
        }
    }
}