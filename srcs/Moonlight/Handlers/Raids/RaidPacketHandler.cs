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
    internal class RaidPacketHandler : PacketHandler<RaidPacket>
    {
        private readonly IEventManager _eventManager;

        public RaidPacketHandler(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        protected override void Handle(Client client, RaidPacket packet)
        {
            if (client.Character == null)
            {
                return;
            }

            Raid raid = client.Character.Raid;

            if (packet.Type == RaidPacketType.Initialization)
            {
                raid = client.Character.Raid = new Raid
                {
                    Status = RaidStatus.Preparation,

                };

                _eventManager.Emit(new RaidInitializedEvent(client)
                {
                    Raid = raid
                });
            }

            if (raid == null)
            {
                return;
            }

            switch (packet.Type)
            {
                case RaidPacketType.RaidLeader:
                    raid.LeaderId = packet.LeaderId;
                    break;
                case RaidPacketType.Start:
                    raid.Status = RaidStatus.InProgress;
                    raid.StartTime = DateTime.Now;

                    _eventManager.Emit(new RaidStatusChangedEvent(client)
                    {
                        Raid = raid
                    });
                    break;
                case RaidPacketType.Left:
                    raid.Status = RaidStatus.Left;
                    _eventManager.Emit(new RaidStatusChangedEvent(client)
                    {
                        Raid = raid
                    });
                    break;
            }
        }
    }
}
