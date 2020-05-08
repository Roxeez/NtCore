using Moonlight.Clients;
using Moonlight.Game.Entities;
using Moonlight.Game.Maps;
using Moonlight.Packet.Map;

namespace Moonlight.Handlers.Maps
{
    internal class GetPacketHandler : PacketHandler<GetPacket>
    {
        protected override void Handle(Client client, GetPacket packet)
        {
            Map map = client.Character?.Map;

            LivingEntity entity = map?.GetEntity<LivingEntity>(packet.EntityType, packet.EntityId);
            GroundItem groundItem = map?.GetEntity<GroundItem>(packet.DropId);

            if (entity == null || groundItem == null)
            {
                return;
            }

            map.RemoveEntity(groundItem);
        }
    }
}