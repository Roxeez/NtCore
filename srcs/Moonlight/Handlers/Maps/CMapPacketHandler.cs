using Moonlight.Clients;
using Moonlight.Core.Logging;
using Moonlight.Event;
using Moonlight.Event.Maps;
using Moonlight.Game.Factory;
using Moonlight.Game.Maps;
using Moonlight.Packet.Map;

namespace Moonlight.Handlers.Maps
{
    internal class CMapPacketHandler : PacketHandler<CMapPacket>
    {
        private readonly IEventManager _eventManager;
        private readonly ILogger _logger;
        private readonly IMapFactory _mapFactory;

        public CMapPacketHandler(ILogger logger, IMapFactory mapFactory, IEventManager eventManager)
        {
            _logger = logger;
            _mapFactory = mapFactory;
            _eventManager = eventManager;
        }

        protected override void Handle(Client client, CMapPacket packet)
        {
            if (!packet.IsBaseMap)
            {
                _logger.Debug("Is base map");
                return;
            }
            
            Map destination = _mapFactory.CreateMap(packet.MapId);
            if (client.Character?.Map?.Id == destination.Id)
            {
                return;
            }

            Map source = client.Character.Map;

            destination.AddEntity(client.Character);

            if (source != null)
            {
                _eventManager.Emit(new MapChangeEvent(client)
                {
                    Character = client.Character,
                    Source = source,
                    Destination = destination
                });
            }
        }
    }
}