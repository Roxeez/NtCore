using Moonlight.Core;
using Moonlight.Core.Enums;
using Moonlight.Database.Dto;
using Moonlight.Game.Maps;
using Moonlight.Tests.Extensions;
using NFluent;
using Xunit;

namespace Moonlight.Tests.Packet.Handling
{
    public class CharacterPacketHandlingTests : PacketHandlingTests
    {
        [Fact]
        public void CMap_Packet_Change_Character_Map()
        {
            Client.ReceivePacket("c_map 1 1 1");

            Check.That(Character.Map).IsNotNull();
            Check.That(Character.Map.Id).Is(1);
            Check.That(Character.Map.Name).Is("NosVille");
        }
        
        [Fact]
        public void Faction_Packet_Update_Character_Faction()
        {
            Client.ReceivePacket("fs 1");

            Check.That(Character.Faction).Is(FactionType.ANGEL);
        }

        [Fact]
        public void Ski_Packet_Set_Character_Skills()
        {
            Client.ReceivePacket("ski 833 833 833 834 835 836 837 838 839 840 841 21 25 37 45 353 356");

            Check.That(Client.Character.Skills).HasElementThatMatches(x => x.Id == 833);
            Check.That(Client.Character.Skills).HasElementThatMatches(x => x.Id == 837);
        }

        [Fact]
        public void Sp_Packet_Update_Character_Sp_Points()
        {
            Client.ReceivePacket("sp 875000 1000000 8500 10000");

            Check.That(Character.SpPoints).Is(8500);
            Check.That(Character.AdditionalSpPoints).Is(875000);
        }

        [Fact]
        public void Stat_Packet_Set_Character_Stat()
        {
            Client.ReceivePacket("stat 2000 3000 1000 1500");

            Check.That(Character.Hp).Is(2000);
            Check.That(Character.MaxHp).Is(3000);
            Check.That(Character.Mp).Is(1000);
            Check.That(Character.MaxMp).Is(1500);
        }

        [Fact]
        public void Walk_Packet_Set_Last_Movement()
        {
            Client.ReceivePacket("walk 140 87 1 12");

            Check.That(Client.Character.Position).Is(new Position(140, 87));
            Check.That(Client.Character.LastMovement).Not.IsDefaultValue();
        }
    }
}