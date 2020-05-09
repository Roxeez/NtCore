using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonlight.Clients;
using Moonlight.Event;
using Moonlight.Event.Raids;
using Moonlight.Game.Entities;
using Moonlight.Game.Raids;
using Moonlight.Packet.Raid;

namespace Moonlight.Handlers.Raids
{
    internal class RaidBossPacketHandler : PacketHandler<RaidBossPacket>
    {
        private readonly IEventManager _eventManager;

        public RaidBossPacketHandler(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        protected override void Handle(Client client, RaidBossPacket packet)
        {
            if (client.Character == null || client.Character.Raid == null)
            {
                return;
            }

            Raid raid = client.Character.Raid;

            if (raid.Status == RaidStatus.Unknown)
            {
                raid.Status = RaidStatus.InProgress;
            }

            Monster boss = client.Character.Map.GetEntity<Monster>(packet.MonsterId);

            if (boss == null)
            {
                return;
            }

            if (raid.Boss == null)
            {
                raid.Boss = boss;
                boss.IsRaidBoss = true;

                raid.Bosses.Add(raid.Boss);

                FireInitialized(client, raid, raid.Boss);
            }
            else if (raid.Boss.Id != packet.MonsterId && !raid.Bosses.Exists(x => x.Id == packet.MonsterId))
            {
                Monster anotherRaidBoss = client.Character.Map.GetEntity<Monster>(packet.MonsterId);
                anotherRaidBoss.IsRaidBoss = true;

                raid.Bosses.Add(anotherRaidBoss);
                FireInitialized(client, raid, anotherRaidBoss);
            }
        }

        protected void FireInitialized(Client client, Raid raid, Monster boss)
        {
            _eventManager.Emit(new RaidBossInitializedEvent(client)
            {
                Raid = raid,
                RaidBoss = boss
            });
        }
    }
}
