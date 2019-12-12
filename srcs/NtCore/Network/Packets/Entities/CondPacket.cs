﻿using NtCore.Enums;

namespace NtCore.Network.Packets.Entities
{
    [PacketInfo("cond", PacketType.Recv)]
    public class CondPacket : Packet
    {
        [PacketIndex(0)]
        public EntityType EntityType { get; set; }

        [PacketIndex(1)]
        public int EntityId { get; set; }

        [PacketIndex(2)]
        public bool IsAttackAllowed { get; set; }

        [PacketIndex(3)]
        public bool IsMovementAllowed { get; set; }

        [PacketIndex(4)]
        public byte Speed { get; set; }
    }
}