using Moonlight.Clients;
using Moonlight.Game.Entities;
using Moonlight.Game.Maps;
using Moonlight.Packet.Entity;

namespace Moonlight.Handlers.Entities
{
    internal class CondPacketHandler : PacketHandler<CondPacket>
    {
        protected override void Handle(Client client, CondPacket packet)
        {
            Map map = client.Character?.Map;

            LivingEntity entity = map?.GetEntity<LivingEntity>(packet.EntityType, packet.EntityId);
            if (entity == null)
            {
                return;
            }

            entity.Speed = packet.Speed;
        }
    }
}