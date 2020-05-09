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
    internal class RaidBfPacketHandler : PacketHandler<RaidBfPacket>
    {
        private readonly IEventManager _eventManager;

        public RaidBfPacketHandler(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        protected override void Handle(Client client, RaidBfPacket packet)
        {
            if (client.Character == null)
            {
                return;
            }

            Raid raid = client.Character.Raid;

            if (raid == null || raid.Ended)
            {
                return;
            }

            if (packet.Type == 1)
            {
                raid.Status = RaidStatus.Successful;
            }
            else if (packet.Type != 0)
            {
                raid.Status = RaidStatus.Fail;
            }

            _eventManager.Emit(new RaidStatusChangedEvent(client)
            {
                Raid = raid
            });
        }
    }
}
