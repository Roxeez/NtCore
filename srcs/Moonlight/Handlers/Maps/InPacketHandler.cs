using System;
using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Core.Enums;
using Moonlight.Core.Logging;
using Moonlight.Event;
using Moonlight.Event.Maps;
using Moonlight.Game.Entities;
using Moonlight.Game.Factory;
using Moonlight.Game.Maps;
using Moonlight.Packet.Map;

namespace Moonlight.Handlers.Maps
{
    internal class InPacketHandler : PacketHandler<InPacket>
    {
        private readonly IEntityFactory _entityFactory;
        private readonly IEventManager _eventManager;
        private readonly ILogger _logger;

        public InPacketHandler(ILogger logger, IEntityFactory entityFactory, IEventManager eventManager)
        {
            _logger = logger;
            _entityFactory = entityFactory;
            _eventManager = eventManager;
        }

        protected override void Handle(Client client, InPacket packet)
        {
            Map map = client.Character?.Map;

            if (map == null)
            {
                _logger.Warn("Handling InPacket but character map is null");
                return;
            }

            if (map.Contains(packet.EntityType, packet.EntityId))
            {
                _logger.Warn($"Entity {packet.EntityType} {packet.EntityId} already on map");
                return;
            }

            Entity entity;
            switch (packet.EntityType)
            {
                case EntityType.MONSTER:
                    entity = _entityFactory.CreateMonster(packet.EntityId, packet.Vnum);
                    break;
                case EntityType.NPC:
                    entity = _entityFactory.CreateNpc(packet.EntityId, packet.Vnum, packet.NpcSubPacket.Name);
                    break;
                case EntityType.PLAYER:
                    entity = _entityFactory.CreatePlayer(packet.EntityId, packet.Name);
                    break;
                case EntityType.GROUND_ITEM:
                    entity = _entityFactory.CreateGroundItem(packet.EntityId, packet.Vnum, packet.DropSubPacket.Amount);
                    break;
                default:
                    throw new InvalidOperationException("Undefined entity type");
            }

            entity.Position = new Position(packet.PositionX, packet.PositionY);

            if (entity is Player player)
            {
                player.Class = packet.PlayerSubPacket.Class;
                player.Gender = packet.PlayerSubPacket.Gender;
                player.Direction = packet.Direction;
                player.HpPercentage = packet.PlayerSubPacket.HpPercentage;
                player.MpPercentage = packet.PlayerSubPacket.MpPercentage;
            }

            if (entity is Monster monster)
            {
                monster.Direction = packet.Direction;
                monster.HpPercentage = packet.NpcSubPacket.HpPercentage;
                monster.MpPercentage = packet.NpcSubPacket.MpPercentage;
                monster.Faction = packet.NpcSubPacket.Faction;
            }

            if (entity is Npc npc)
            {
                npc.Direction = packet.Direction;
                npc.HpPercentage = packet.NpcSubPacket.HpPercentage;
                npc.MpPercentage = packet.NpcSubPacket.MpPercentage;
                npc.Faction = packet.NpcSubPacket.Faction;
            }

            if (entity is GroundItem drop)
            {
                drop.Owner = map.GetEntity<Player>(packet.DropSubPacket.Owner);
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