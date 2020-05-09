using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonlight.Game.Entities;
using Moonlight.Packet.Raid;

namespace Moonlight.Game.Raids
{
    public class 
        Raid
    {
        public Raid()
        {
            Players = new List<RaidPlayerData>();
            Bosses = new List<Monster>();
        }

        public bool Ended => Status == RaidStatus.Fail || Status == RaidStatus.Left || Status == RaidStatus.Successful;

        public int? RaidId { get; set; }

        public long LeaderId { get; set; }

        public int MinimumLevel { get; set; }

        public int MaximumLevel { get; set; }

        public DateTime? StartTime { get; set; }

        public RaidStatus Status { get; set; }

        public List<RaidPlayerData> Players { get; set; }

        public Monster Boss { get; set; }

        public List<Monster> Bosses { get; set; }

        public bool MultipleBossRaid => (Bosses?.Count ?? 0) > 1;

        public int MonsterLockerInitial { get; set; }

        public int MonsterLockerCurrent { get; set; }

        public int ButtonLockerInitial { get; set; }

        public int ButtonLockerCurrent { get; set; }

        public int CurrentLifes { get; set; }

        public int InitialLifes { get; set; }
    }
}
