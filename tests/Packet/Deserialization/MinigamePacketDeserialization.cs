using Moonlight.Packet.Core.Serialization;
using Moonlight.Packet.Map.Miniland.Minigame;
using Moonlight.Tests.Extensions;
using Moonlight.Tests.Utility;
using NFluent;
using Xunit;

namespace Moonlight.Tests.Packet.Deserialization
{
    public class MinigamePacketDeserialization
    {
        private readonly IDeserializer _deserializer;

        public MinigamePacketDeserialization()
        {
            _deserializer = TestHelper.CreateDeserializer();
        }

        [Fact]
        public void Mg_Packet()
        {
            MgPacket packet = _deserializer.Deserialize<MgPacket>("mg 1 7");
            
            Check.That(packet.Type).Is<byte>(1);
            Check.That(packet.MinigameId).Is<short>(7);
        }

        [Fact]
        public void Mlo_Info_Packet()
        {
            MloInfoPacket packet = _deserializer.Deserialize<MloInfoPacket>("mlo_info 1 3120 7 2000 0 0 0 999 1000 3999 4000 7999 8000 11999 12000 19999 20000 1000000");

            Check.That(packet.Points).Is<short>(2000);
            Check.That(packet.Vnum).Is(3120);
            Check.That(packet.ObjectId).Is(7);
        }

        [Fact]
        public void Mlo_Lv_Packet()
        {
            MloLvPacket packet = _deserializer.Deserialize<MloLvPacket>("mlo_lv 4");
            
            Check.That(packet.Level).Is(4);
        }

        [Fact]
        public void Mlo_Rw_Packet()
        {
            MloRwPacket packet = _deserializer.Deserialize<MloRwPacket>("mlo_rw 2070 4");

            Check.That(packet.ItemVnum).Is(2070);
            Check.That(packet.Count).Is(4);
        }

        [Fact]
        public void Mlo_St_Packet()
        {
            MloStPacket packet = _deserializer.Deserialize<MloStPacket>("mlo_st 3");
            
            Check.That(packet.Header).Is("mlo_st");
        }

        [Fact]
        public void MlPt_Packet()
        {
            MlPtPacket packet = _deserializer.Deserialize<MlPtPacket>("mlpt 2000 2000");
            Check.That(packet.Points).Is<short>(2000);
        }
    }
}