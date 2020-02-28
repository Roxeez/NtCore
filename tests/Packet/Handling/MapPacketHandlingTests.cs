using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Database.Dto;
using Moonlight.Game.Entities;
using Moonlight.Game.Maps;
using Moonlight.Tests.Extensions;
using NFluent;
using Xunit;

namespace Moonlight.Tests.Packet.Handling
{
    public class MapPacketHandlingTests : PacketHandlingTests
    {
        [Fact]
        public void In_Packet_Add_Drop_To_Map()
        {
            Map map = Character.Map;

            Client.ReceivePacket("in 9 503 4808544 82 4 1 0 0 -1");

            GroundItem groundItem = map.GetEntity<GroundItem>(4808544);

            Check.That(groundItem).IsNotNull();
            Check.That(groundItem.Item.Vnum).Is(503);
            Check.That(groundItem.Owner).IsNull();
            Check.That(groundItem.Position).Is(new Position(82, 4));
            Check.That(groundItem.Amount).Is(1);
        }

        [Fact]
        public void In_Packet_Add_Npc_To_Map()
        {
            Map map = Character.Map;

            Client.ReceivePacket("in 2 334 2206 96 57 2 87 100 0 0 0 -1 1 0 -1 - 2 -1 0 0 0 0 0 0 0 0");

            Npc npc = map.GetEntity<Npc>(2206);

            Check.That(npc).IsNotNull();
            Check.That(npc.Vnum).Is(334);
            Check.That(npc.Position.X).Is<short>(96);
            Check.That(npc.Position.Y).Is<short>(57);
            Check.That(npc.Direction).Is<byte>(2);
            Check.That(npc.HpPercentage).Is<byte>(87);
            Check.That(npc.MpPercentage).Is<byte>(100);
        }

        [Fact]
        public void In_Packet_Add_Pet_To_Map()
        {
            Map map = Character.Map;

            Client.ReceivePacket("in 2 815 749159 75 64 2 100 100 0 0 3 1290807 1 0 -1 Ratufu^centurion 2 -1 0 0 0 0 0 0 0 0");

            Npc npc = map.GetEntity<Npc>(749159);

            Check.That(npc).IsNotNull();
            Check.That(npc.Vnum).Is(815);
            Check.That(npc.Position).Is(new Position(75, 64));
            Check.That(npc.Direction).Is<byte>(2);
            Check.That(npc.HpPercentage).Is<byte>(100);
            Check.That(npc.MpPercentage).Is<byte>(100);
            Check.That(npc.Name).Is("Ratufu centurion");
        }

        [Fact]
        public void In_Packet_Add_Player_To_Map()
        {
            Map map = Character.Map;

            Client.ReceivePacket(
                "in 1 ~Orage~ - 652158 100 58 2 0 0 0 53 1 204.4949.4964.4955.8026.4130.8205.4179.-1.4443 100 100 0 28 4 2 0 41 0 20 87 87 2582 100ßlaze(Membre) 24 0 15 0 12 92 8 0|0|0 0 35 10 28 93733");

            Player player = map.GetEntity<Player>(652158);

            Check.That(player).IsNotNull();
            Check.That(player.Name).Is("~Orage~");
            Check.That(player.Position).Is(new Position(100, 58));
            Check.That(player.Direction).Is<byte>(2);
            Check.That(player.HpPercentage).Is<byte>(100);
            Check.That(player.MpPercentage).Is<byte>(100);
        }

        [Fact]
        public void Out_Packet_Remove_Entity_From_Map()
        {
            Player player = EntityFactory.CreatePlayer(128476, "player");

            Map map = Character.Map;
            map.AddEntity(player);

            Client.ReceivePacket($"out {player.EntityType} {player.Id}");

            Check.That(map.Players).Not.Contains(player);
        }

        [Fact]
        public void Get_Packet_Remove_Entity()
        {
            GroundItem item = EntityFactory.CreateGroundItem(123456, 1, 1);

            Map map = Character.Map;
            map.AddEntity(item);
            
            Client.ReceivePacket("get 9 123456");
        }
    }
}