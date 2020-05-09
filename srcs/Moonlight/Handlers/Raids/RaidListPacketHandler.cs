using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonlight.Clients;
using Moonlight.Event;
using Moonlight.Event.Raids;
using Moonlight.Game.Raids;
using Moonlight.Packet.Raid;

namespace Moonlight.Handlers.Raids
{
    internal class RaidListPacketHandler : PacketHandler<RaidListPacket>
    {
        private readonly IEventManager _eventManager;

        public RaidListPacketHandler(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        protected override void Handle(Client client, RaidListPacket packet)
        {
            if (client.Character == null || client.Character.Raid == null)
            {
                return;
            }

            Raid raid = client.Character.Raid;

            if (raid.RaidId == null)
            {
                raid.RaidId = packet.RaidId;
                raid.MinimumLevel = packet.MinimumLevel;
                raid.MaximumLevel = packet.MaximumLevel;

                _eventManager.Emit(new RaidInfoRetrievedEvent(client)
                {
                    Raid = raid
                });
            }

            raid.Players.AddRange(packet.Data.Except(raid.Players));
            raid.Players.RemoveAll(x => !packet.Data.Contains(x));
        }
    }
}
