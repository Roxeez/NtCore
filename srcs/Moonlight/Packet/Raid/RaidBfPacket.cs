using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonlight.Packet.Core.Attributes;

namespace Moonlight.Packet.Raid
{
    [PacketHeader("raidbf")]
    internal class RaidBfPacket : Packet
    {
        [PacketIndex(1)]
        public int Type { get; set; }
    }
}
