using Moonlight.Clients;
using Moonlight.Packet.Character;

namespace Moonlight.Handlers.Characters
{
    internal class GoldPacketHandler : PacketHandler<GoldPacket>
    {
        protected override void Handle(Client client, GoldPacket packet)
        {
            if (client.Character != null)
            {
                client.Character.Gold = packet.Gold;
            }
        }
    }
}