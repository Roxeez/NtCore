using System.Linq;
using Moonlight.Clients;
using Moonlight.Game.Battle;
using Moonlight.Packet.Battle;

namespace Moonlight.Handlers.Battle
{
    internal class SrPacketHandler : PacketHandler<SrPacket>
    {
        protected override void Handle(Client client, SrPacket packet)
        {
            if (client.Character == null || client.Character.Skills == null)
            {
                return;
            }

            Skill skill = client.Character.Skills.FirstOrDefault(x => x.CastId == packet.CastId);
            if (skill == null)
            {
                return;
            }

            skill.IsOnCooldown = false;
        }
    }
}