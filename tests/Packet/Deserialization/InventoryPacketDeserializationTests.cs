using Moonlight.Core.Enums;
using Moonlight.Packet.Character.Inventory;
using Moonlight.Packet.Core.Serialization;
using Moonlight.Tests.Extensions;
using Moonlight.Tests.Utility;
using NFluent;
using Xunit;

namespace Moonlight.Tests.Packet.Deserialization
{
    public class InventoryPacketDeserializationTests
    {
        public InventoryPacketDeserializationTests() => _deserializer = TestHelper.CreateDeserializer();

        private readonly IDeserializer _deserializer;

        [Fact]
        public void Inv_Packet_Costume_Bag()
        {
            InvPacket packet = _deserializer.Deserialize<InvPacket>("inv 7 0.8101.0.72.0 1.8117.0.0.0 2.8192.0.0.0");

            Check.That(packet.BagType).Is(BagType.COSTUME);

            Check.That(packet.SubPackets).CountIs(3);
            Check.That(packet.SubPackets).HasElementAt(0).WhichMatch(x => x.Slot == 0 && x.VNum == 8101);
            Check.That(packet.SubPackets).HasElementAt(1).WhichMatch(x => x.Slot == 1 && x.VNum == 8117);
            Check.That(packet.SubPackets).HasElementAt(2).WhichMatch(x => x.Slot == 2 && x.VNum == 8192);
        }

        [Fact]
        public void Inv_Packet_Equipment_Bag()
        {
            InvPacket packet = _deserializer.Deserialize<InvPacket>("inv 0 0.8112.0.0.0.0 1.8114.0.0.0.0 2.8111.0.0.0.0");

            Check.That(packet.BagType).Is(BagType.EQUIPMENT);

            Check.That(packet.SubPackets).CountIs(3);
            Check.That(packet.SubPackets).HasElementAt(0).WhichMatch(x => x.Slot == 0 && x.VNum == 8112);
            Check.That(packet.SubPackets).HasElementAt(1).WhichMatch(x => x.Slot == 1 && x.VNum == 8114);
            Check.That(packet.SubPackets).HasElementAt(2).WhichMatch(x => x.Slot == 2 && x.VNum == 8111);
        }

        [Fact]
        public void Inv_Packet_Etc_Bag()
        {
            InvPacket packet = _deserializer.Deserialize<InvPacket>("inv 2 0.2801.9 1.2800.2");

            Check.That(packet.BagType).Is(BagType.ETC);

            Check.That(packet.SubPackets).CountIs(2);
            Check.That(packet.SubPackets).HasElementAt(0).WhichMatch(x => x.Slot == 0 && x.VNum == 2801 && x.RareAmount == 9);
            Check.That(packet.SubPackets).HasElementAt(1).WhichMatch(x => x.Slot == 1 && x.VNum == 2800 && x.RareAmount == 2);
        }

        [Fact]
        public void Inv_Packet_Main_Bag()
        {
            InvPacket packet = _deserializer.Deserialize<InvPacket>("inv 1 1.1012.23 3.1027.480 4.1211.17");

            Check.That(packet.BagType).Is(BagType.MAIN);

            Check.That(packet.SubPackets).CountIs(3);
            Check.That(packet.SubPackets).HasElementAt(0).WhichMatch(x => x.Slot == 1 && x.VNum == 1012 && x.RareAmount == 23);
            Check.That(packet.SubPackets).HasElementAt(1).WhichMatch(x => x.Slot == 3 && x.VNum == 1027 && x.RareAmount == 480);
            Check.That(packet.SubPackets).HasElementAt(2).WhichMatch(x => x.Slot == 4 && x.VNum == 1211 && x.RareAmount == 17);
        }

        [Fact]
        public void Inv_Packet_Miniland_Bag()
        {
            InvPacket packet = _deserializer.Deserialize<InvPacket>("inv 3 0.3104.1 1.3157.1");

            Check.That(packet.BagType).Is(BagType.MINILAND);

            Check.That(packet.SubPackets).CountIs(2);
            Check.That(packet.SubPackets).HasElementAt(0).WhichMatch(x => x.Slot == 0 && x.VNum == 3104 && x.RareAmount == 1);
            Check.That(packet.SubPackets).HasElementAt(1).WhichMatch(x => x.Slot == 1 && x.VNum == 3157 && x.RareAmount == 1);
        }

        [Fact]
        public void Inv_Packet_Specialist_Bag()
        {
            InvPacket packet = _deserializer.Deserialize<InvPacket>("inv 6");

            Check.That(packet.BagType).Is(BagType.SPECIALIST);
            Check.That(packet.SubPackets).CountIs(0);
        }

        [Fact]
        public void Ivn_Packet()
        {
            IvnPacket packet = _deserializer.Deserialize<IvnPacket>("ivn 1 13.9033.5.0");

            Check.That(packet.BagType).Is(BagType.MAIN);
            Check.That(packet.SubPacket.Slot).Is(13);
            Check.That(packet.SubPacket.VNum).Is(9033);
            Check.That(packet.SubPacket.RareAmount).Is(5);
        }
    }
}