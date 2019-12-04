﻿using NtCore.API.Client;
using NtCore.API.Enums;
using NtCore.API.Game.Entities;
using NtCore.Extensions;
using NtCore.Game.Entities;
using NtCore.Network.Packets.Character;

namespace NtCore.Network.Handlers.Character
{
    public class CondPacketHandler : PacketHandler<CondPacket>
    {
        public override void Handle(IClient client, CondPacket packet)
        {
            ILivingEntity entity = client.Character.Map.GetLivingEntity(packet.EntityType, packet.EntityId);

            switch (packet.EntityType)
            {
                case EntityType.Monster:
                    entity.AsModifiable<Monster>().Speed = packet.Speed;
                    break;
                case EntityType.Npc:
                    entity.AsModifiable<Npc>().Speed = packet.Speed;
                    break;
                case EntityType.Player:
                    entity.AsModifiable<Player>().Speed = packet.Speed;
                    break;
            }
        }
    }
}