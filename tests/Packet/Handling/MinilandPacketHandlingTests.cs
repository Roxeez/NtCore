using Moonlight.Core;
using Moonlight.Game.Maps;
using Moonlight.Tests.Extensions;
using NFluent;
using Xunit;

namespace Moonlight.Tests.Packet.Handling
{
    public class MinilandPacketHandlingTests : PacketHandlingTests
    {
        [Fact]
        public void MlInfoBr_Packet_Set_Miniland_Owner()
        {
            Miniland miniland = MapFactory.CreateMiniland();
            miniland.AddEntity(Character);

            Client.ReceivePacket("mlinfobr 3800 *bliblou* 2 343 10 Cc^tlm");

            Check.That(miniland.Owner).Is("*bliblou*");
        }

        [Fact]
        public void MltObj_Packet_Set_Miniland_Objects()
        {
            Miniland miniland = MapFactory.CreateMiniland();
            miniland.AddEntity(Character);

            Client.ReceivePacket("mltobj 3210.5.17.2 3005.6.31.3 3187.7.7.14");

            Check.That(miniland.Objects).CountIs(3);
            Check.That(miniland.Objects).HasElementAt(0).WhichMatch(x => x.Item.Vnum == 3210 && x.Position.Equals(new Position(17, 2)) && x.Slot == 5);
            Check.That(miniland.Objects).HasElementAt(1).WhichMatch(x => x.Item.Vnum == 3005 && x.Position.Equals(new Position(31, 3)) && x.Slot == 6);
            Check.That(miniland.Objects).HasElementAt(2).WhichMatch(x => x.Item.Vnum == 3187 && x.Position.Equals(new Position(7, 14)) && x.Slot == 7);
        }
    }
}