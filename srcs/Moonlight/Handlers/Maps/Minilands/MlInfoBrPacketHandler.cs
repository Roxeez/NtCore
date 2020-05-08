using Moonlight.Clients;
using Moonlight.Core.Logging;
using Moonlight.Game.Maps;
using Moonlight.Packet.Map.Miniland;

namespace Moonlight.Handlers.Maps.Minilands
{
    internal class MlInfoBrPacketHandler : PacketHandler<MlInfoBrPacket>
    {
        private readonly ILogger _logger;

        public MlInfoBrPacketHandler(ILogger logger) => _logger = logger;

        protected override void Handle(Client client, MlInfoBrPacket packet)
        {
            var miniland = client.Character?.Map as Miniland;
            if (miniland == null)
            {
                _logger.Warn("Receiving packet but current map is not a miniland");
                return;
            }

            miniland.Owner = packet.Owner;
        }
    }
}