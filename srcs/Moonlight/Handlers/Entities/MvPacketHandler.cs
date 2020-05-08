using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Event;
using Moonlight.Event.Entities;
using Moonlight.Game.Entities;
using Moonlight.Packet.Entity;

namespace Moonlight.Handlers.Entities
{
    internal class MvPacketHandler : PacketHandler<MvPacket>
    {
        private readonly IEventManager _eventManager;

        public MvPacketHandler(IEventManager eventManager) => _eventManager = eventManager;

        protected override void Handle(Client client, MvPacket packet)
        {
            LivingEntity entity = client.Character?.Map?.GetEntity<LivingEntity>(packet.EntityType, packet.EntityId);

            if (entity == null)
            {
                return;
            }

            Position from = entity.Position;

            entity.Position = new Position(packet.PositionX, packet.PositionY);
            entity.Speed = packet.Speed;

            _eventManager.Emit(new EntityMoveEvent(client)
            {
                Entity = entity,
                From = from,
                To = entity.Position
            });
        }
    }
}