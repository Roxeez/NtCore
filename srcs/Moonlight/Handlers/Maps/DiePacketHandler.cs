using Moonlight.Clients;
using Moonlight.Core.Logging;
using Moonlight.Game.Entities;
using Moonlight.Game.Maps;
using Moonlight.Packet.Map;

namespace Moonlight.Handlers.Maps
{
    internal class DiePacketHandler : PacketHandler<DiePacket>
    {
        private readonly ILogger _logger;

        public DiePacketHandler(ILogger logger) => _logger = logger;

        protected override void Handle(Client client, DiePacket packet)
        {
            Map map = client.Character?.Map;
            if (map == null)
            {
                return;
            }

            LivingEntity entity = map.GetEntity<LivingEntity>(packet.EntityType, packet.EntityId);
            if (entity == null)
            {
                _logger.Info($"Can't found entity {packet.EntityType} {packet.EntityId} in map (Die)");
                return;
            }

            entity.HpPercentage = 0;
            map.RemoveEntity(entity);

            _logger.Info($"Entity {entity.EntityType} {entity.Id} died");
        }
    }
}