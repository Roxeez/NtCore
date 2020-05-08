using System;
using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Game.Entities;
using Moonlight.Packet.Character;

namespace Moonlight.Handlers.Characters
{
    internal class WalkPacketHandler : PacketHandler<WalkPacket>
    {
        protected override void Handle(Client client, WalkPacket packet)
        {
            Character character = client.Character;

            if (character != null)
            {
                character.Speed = packet.Speed;
                character.Position = new Position(packet.PositionX, packet.PositionY);
                character.LastMovement = DateTime.Now;
            }
        }
    }
}