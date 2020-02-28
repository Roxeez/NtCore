using Moonlight.Packet.Core.Serialization;
using Moonlight.Packet.Map.Miniland;
using Moonlight.Tests.Extensions;
using Moonlight.Tests.Utility;
using NFluent;
using Xunit;

namespace Moonlight.Tests.Packet.Deserialization
{
    public class MinilandPacketDeserializationTests
    {
        public MinilandPacketDeserializationTests() => _deserializer = TestHelper.CreateDeserializer();

        private readonly IDeserializer _deserializer;

        [Fact]
        public void MlInfo_Packet()
        {
            MlInfoPacket packet = _deserializer.Deserialize<MlInfoPacket>("mlinfo 0 2000");

            Check.That(packet.Points).Is<short>(2000);
        }

        [Fact]
        public void MlInfoBr_Packet()
        {
            MlInfoBrPacket packet = _deserializer.Deserialize<MlInfoBrPacket>("mlinfobr 3800 *bliblou* 2 343 10 Cc^tlm");

            Check.That(packet.Owner).Is("*bliblou*");
        }

        [Fact]
        public void MlObjLstPacket()
        {
            MlObjLstPacket packet = _deserializer.Deserialize<MlObjLstPacket>("mlobjlst 0.0.0.0.4.2.0.100.0.0 1.0.0.0.0.0.0.100.0.0");

            Check.That(packet.Header).Is("mlobjlst");
            // TODO : Not implemented yet
        }

        [Fact]
        public void MltObjPacket()
        {
            MltObjPacket packet = _deserializer.Deserialize<MltObjPacket>("mltobj 3210.5.17.2 3005.6.31.3 3187.7.7.14");

            Check.That(packet.Header).Is("mltobj");
            // TODO : Not implemented yet
        }
    }
}