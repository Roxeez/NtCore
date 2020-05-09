using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonlight.Packet.Core.Attributes;

namespace Moonlight.Packet.Raid
{
    [PacketHeader("rdlst")]
    internal class RaidListPacket : Packet
    {
        public int MinimumLevel { get; set; }
        public int MaximumLevel { get; set; }
        public int RaidId { get; set; }
        public List<RaidPlayerData> Data { get; set; }
    }
}
