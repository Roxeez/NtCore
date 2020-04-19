using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Core.Enums;
using Moonlight.Core.Logging;
using Moonlight.Event;
using Moonlight.Event.Maps;
using Moonlight.Game.Entities;
using Moonlight.Game.Factory;
using Moonlight.Game.Inventories.Items;
using Moonlight.Game.Maps;
using Moonlight.Packet.Map;
using System;

namespace Moonlight.Handlers.Maps
{
    internal class DropPacketHandler : PacketHandler<DropPacket>
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IEventManager _eventManager;
        private readonly ILogger _logger;

        public DropPacketHandler(ILogger logger, IEntityFactory entityFactory, IEventManager eventManager)
        {
            _logger = logger;
            _entityFactory = entityFactory;
            _eventManager = eventManager;
        }
        protected override void Handle(Client client, DropPacket packet)
        {
            Map map = client.Character.Map;

            if (map == null)
            {
                _logger.Warn("Handling InPacket but character map is null");
                return;
            }

            if(map.Contains(EntityType.GROUND_ITEM, packet.VNum))
            {
                _logger.Warn($"Entity {EntityType.GROUND_ITEM} {packet.VNum} already on map");
                return;
            }

            Entity entity = _entityFactory.CreateGroundItem(packet.DropId, packet.VNum, packet.Amount);
            entity.Position = new Position(packet.PositionX, packet.PositionY);

            if (entity is GroundItem drop)
            {
                drop.Owner = map.GetEntity<Player>(packet.OwnerId);
            }

            map.AddEntity(entity);
            _logger.Info($"Entity {entity.EntityType} {entity.Id} joined map");

            _eventManager.Emit(new EntityJoinEvent(client)
            {
                Map = map,
                Entity = entity
            });
        }
    }
}