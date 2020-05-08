using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Game.Entities;
using Moonlight.Game.Factory;
using Moonlight.Game.Maps;
using Moonlight.Packet.Character;

namespace Moonlight.Handlers.Characters
{
    internal class AtPacketHandler : PacketHandler<AtPacket>
    {
        private readonly IMapFactory _mapFactory;

        public AtPacketHandler(IMapFactory mapFactory) => _mapFactory = mapFactory;

        protected override void Handle(Client client, AtPacket packet)
        {
            if (client.Character == null)
            {
                return;
            }

            Character character = client.Character;
            if (packet.CharacterId != character.Id)
            {
                return;
            }

            Map map = _mapFactory.CreateMap(packet.MapId);
            map.AddEntity(character);
            character.Position = new Position(packet.PositionX, packet.PositionY);
        }
    }
}