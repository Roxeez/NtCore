using Moonlight.Clients;
using Moonlight.Core.Logging;
using Moonlight.Game.Maps;
using Moonlight.Packet.Map.Miniland;

namespace Moonlight.Handlers.Maps.Minilands
{
    internal class MlInfoPacketHandler : PacketHandler<MlInfoPacket>
    {
        protected override void Handle(Client client, MlInfoPacket packet)
        {
            client.Character.ProductionPoints = packet.Points;
            
            var miniland = client.Character?.Map as Miniland;
            if (miniland == null)
            {
                return;
            }

            miniland.Owner = client.Character.Name;
        }
    }
}