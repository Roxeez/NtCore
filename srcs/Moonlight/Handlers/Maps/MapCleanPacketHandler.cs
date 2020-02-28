using Moonlight.Clients;
using Moonlight.Event;
using Moonlight.Event.Maps;
using Moonlight.Packet.Map;

namespace Moonlight.Handlers.Maps
{
    internal class MapCleanPacketHandler : PacketHandler<MapCleanPacket>
    {
        private readonly IEventManager _eventManager;

        public MapCleanPacketHandler(IEventManager eventManager) => _eventManager = eventManager;

        protected override void Handle(Client client, MapCleanPacket packet)
        {
            _eventManager.Emit(new MapCleanEvent(client));
        }
    }
}