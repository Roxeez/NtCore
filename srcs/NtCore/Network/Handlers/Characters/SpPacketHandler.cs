﻿using NtCore.API.Client;
using NtCore.Extensions;
using NtCore.Game.Entities;
using NtCore.Network.Packets.Characters;

namespace NtCore.Network.Handlers.Characters
{
    public class SpPacketHandler : PacketHandler<SpPacket>
    {
        public override void Handle(IClient client, SpPacket packet)
        {
            var character = client.Character.AsModifiable<Character>();

            character.MaximumSpPoints = packet.MaximumPoints;
            character.MaximumAdditionalSpPoints = packet.MaximumAdditionalPoints;

            character.SpPoints = packet.Points;
            character.AdditionalSpPoints = packet.AdditionalPoints;
        }
    }
}