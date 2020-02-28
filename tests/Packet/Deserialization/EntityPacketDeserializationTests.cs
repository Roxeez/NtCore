using Moonlight.Core.Enums;
using Moonlight.Packet.Core.Serialization;
using Moonlight.Packet.Entity;
using Moonlight.Tests.Extensions;
using Moonlight.Tests.Utility;
using NFluent;
using Xunit;

namespace Moonlight.Tests.Packet.Deserialization
{
    public class EntityPacketDeserializationTests
    {
        public EntityPacketDeserializationTests() => _deserializer = TestHelper.CreateDeserializer();

        private readonly IDeserializer _deserializer;

        [Fact]
        public void Cond_Packet()
        {
            CondPacket packet = _deserializer.Deserialize<CondPacket>("cond 1 1283965 0 0 12");

            Check.That(packet.EntityType).Is(EntityType.PLAYER);
            Check.That(packet.EntityId).Is(1283965);
            Check.That(packet.IsAttackAllowed).IsFalse();
            Check.That(packet.IsMovementAllowed).IsFalse();
            Check.That(packet.Speed).Is<byte>(12);
        }

        [Fact]
        public void Mv_Packet()
        {
            MvPacket packet = _deserializer.Deserialize<MvPacket>("mv 3 2107 19 7 5");

            Check.That(packet.EntityType).Is(EntityType.MONSTER);
            Check.That(packet.EntityId).Is(2107);
            Check.That(packet.PositionX).Is<short>(19);
            Check.That(packet.PositionY).Is<short>(7);
            Check.That(packet.Speed).Is<byte>(5);
        }

        [Fact]
        public void Rest_Packet()
        {
            RestPacket packet = _deserializer.Deserialize<RestPacket>("rest 2 782044 0");

            Check.That(packet.EntityType).Is(EntityType.NPC);
            Check.That(packet.EntityId).Is(782044);
            Check.That(packet.IsResting).IsFalse();
        }
    }
}