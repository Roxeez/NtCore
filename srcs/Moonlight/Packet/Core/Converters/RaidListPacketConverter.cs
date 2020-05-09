using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonlight.Core.Enums;
using Moonlight.Packet.Raid;
using Moonlight.Utility.Conversion;
using Moonlight.Utility.Conversion.Converters;

namespace Moonlight.Packet.Core.Converters
{
    internal class RaidListPacketConverter : Converter<RaidListPacket>
    {
        protected override RaidListPacket ToObject(string value, Type type, IConversionFactory factory)
        {
            string[] split = value.Split(' ');

            int minimumLevel = Convert.ToInt32(split[0]);
            int maximumLevel = Convert.ToInt32(split[1]);
            int raidId = Convert.ToInt32(split[2]);
            var playerData = new List<RaidPlayerData>();

            for (int i = 3; i < split.Length; i++)
            {
                string player = split[i];
                string[] splittedPlayer = player.Split('.');

                int level = Convert.ToInt32(splittedPlayer[0]);
                var classType = (ClassType)Convert.ToInt32(splittedPlayer[2]);
                string name = splittedPlayer[4];
                int championLevel = Convert.ToInt32(splittedPlayer[7]);
                int id = Convert.ToInt32(splittedPlayer[6]);

                playerData.Add(new RaidPlayerData
                {
                    Level = level,
                    ChampionLevel = championLevel,
                    Id = id,
                    Name = name,
                    Class = classType
                });
            }

            return new RaidListPacket
            {
                Data = playerData,
                MaximumLevel = maximumLevel,
                MinimumLevel = minimumLevel,
                RaidId = raidId
            };
        }

        protected override string ToString(RaidListPacket value, Type type, IConversionFactory factory) => throw new NotImplementedException();
    }
}
