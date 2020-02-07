﻿using Moonlight.Packet.Core.Attributes;

namespace Moonlight.Packet.Character
{
    [PacketHeader("gold")]
    internal class GoldPacket : Packet
    {
        [PacketIndex(0)]
        public int Gold { get; set; }
    }
}