 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonlight.Clients;
using Moonlight.Game.Raids;
using Moonlight.Packet.Raid;

namespace Moonlight.Handlers.Raids
{
    internal class RaidMbfPacketHandler : PacketHandler<RaidMbfPacket>
    {
        protected override void Handle(Client client, RaidMbfPacket packet)
        {
            if (client.Character == null || client.Character.Raid == null)
            {
                return;
            }

            Raid raid = client.Character.Raid;

            raid.ButtonLockerCurrent = packet.ButtonLockerCurrent;
            raid.ButtonLockerInitial = packet.ButtonLockerInitial;
            raid.MonsterLockerCurrent = packet.MonsterLockerCurrent;
            raid.MonsterLockerInitial = packet.MonsterLockerInitial;
            raid.CurrentLifes = packet.CurrentLifes;
            raid.InitialLifes = packet.InitialLifes;
        }
    }
}
