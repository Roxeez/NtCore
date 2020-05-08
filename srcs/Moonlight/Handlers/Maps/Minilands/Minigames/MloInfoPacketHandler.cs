using System;
using System.Linq;
using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Event;
using Moonlight.Game.Maps;
using Moonlight.Packet.Map.Miniland.Minigame;

namespace Moonlight.Handlers.Maps.Minilands.Minigames
{
    internal class MloInfoPacketHandler : PacketHandler<MloInfoPacket>
    {
        private readonly IEventManager _eventManager;

        public MloInfoPacketHandler(IEventManager eventManager) => _eventManager = eventManager;

        protected override void Handle(Client client, MloInfoPacket packet)
        {
            var miniland = client?.Character?.Map as Miniland;

            var minigame = miniland?.Objects.FirstOrDefault(x => x.Item.Vnum == packet.Vnum && x.Slot == packet.ObjectId) as Minigame;
            if (minigame == null)
            {
                return;
            }

            minigame.Scores.Clear();

            string[] content = packet.Content.Split(' ').Skip(8).ToArray();
            for (int i = 0; i < content.Length; i += 2)
            {
                int minimum = Convert.ToInt32(content[i]);
                int maximum = Convert.ToInt32(content[i + 1]);

                if (maximum > 100000)
                {
                    maximum = minimum + 5000;
                }

                minigame.Scores.Add(new Range(minimum, maximum));
            }
        }
    }
}