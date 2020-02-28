using Moonlight.Core;
using Moonlight.Database.Dto;
using Moonlight.Game.Entities;
using Moonlight.Game.Maps;
using Moonlight.Tests.Extensions;
using NFluent;
using Xunit;

namespace Moonlight.Tests.Packet.Handling
{
    public class EntityPacketHandlingTests : PacketHandlingTests
    {
        [Fact]
        public void Cond_Packet_Change_Entity_Speed()
        {
            Map map = Character.Map;
            Monster monster = EntityFactory.CreateMonster(55, 1);
            
            map.AddEntity(monster);
            
            Client.ReceivePacket("cond 3 55 0 0 10");

            Check.That(monster.Speed).Is<byte>(10);
        }

        [Fact]
        public void Mv_Packet_Change_Entity_Position()
        {
            Map map = Character.Map;
            Npc npc = EntityFactory.CreateNpc(2195, 1, "npc");
            
            map.AddEntity(npc);
            
            Client.ReceivePacket("mv 2 2195 27 109 5");
            
            Check.That(npc.Position).Is(new Position(27, 109));
            Check.That(npc.Speed).Is<byte>(5);
        }
    }
}