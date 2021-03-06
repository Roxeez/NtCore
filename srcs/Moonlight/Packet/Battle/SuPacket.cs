﻿using Moonlight.Core.Enums;
using Moonlight.Packet.Core.Attributes;

namespace Moonlight.Packet.Battle
{
    [PacketHeader("su")]
    internal class SuPacket : Packet
    {
        [PacketIndex(0)]
        public EntityType EntityType { get; set; }

        [PacketIndex(1)]
        public int EntityId { get; set; }

        [PacketIndex(2)]
        public EntityType TargetEntityType { get; set; }

        [PacketIndex(3)]
        public int TargetEntityId { get; set; }

        [PacketIndex(4)]
        public int SkillVnum { get; set; }

        [PacketIndex(10)]
        public bool TargetIsAlive { get; set; }

        [PacketIndex(11)]
        public byte TargetHpPercentage { get; set; }

        [PacketIndex(12)]
        public int Damage { get; set; }
    }
}