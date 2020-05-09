using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moonlight.Packet.Raid
{
    public enum RaidPacketType
    {
        Unknown,
        Initialization,
        Start,
        PlayerHealths,
        RaidLeader,
        Left
    }
}
