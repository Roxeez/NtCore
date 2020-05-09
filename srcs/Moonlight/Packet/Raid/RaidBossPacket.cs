using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonlight.Packet.Core.Attributes;

namespace Moonlight.Packet.Raid
{
    [PacketHeader("rboss")]
    internal class RaidBossPacket : Packet
    {

        [PacketIndex(1)]
        public int MonsterId { get; set; }

        [PacketIndex(2)]
        public int Hp { get; set; }

        [PacketIndex(3)]
        public int MaximumHp { get; set; }

        [PacketIndex(4)]
        public int Vnum { get; set; }
    }
}
