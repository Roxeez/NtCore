using Moonlight.Core.Enums;
using Moonlight.Packet.Core.Attributes;

namespace Moonlight.Packet.Map
{
    [PacketHeader("get")]
    internal class GetPacket : Packet
    {
        [PacketIndex(0)]
        public EntityType EntityType { get; set; }

        [PacketIndex(1)]
        public long EntityId { get; set; }

        [PacketIndex(2)]
        public long DropId { get; set; }
    }
}