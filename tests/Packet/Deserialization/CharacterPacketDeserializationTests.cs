using Moonlight.Core.Enums;
using Moonlight.Packet.Character;
using Moonlight.Packet.Core.Serialization;
using Moonlight.Packet.Map;
using Moonlight.Tests.Extensions;
using Moonlight.Tests.Utility;
using NFluent;
using Xunit;

namespace Moonlight.Tests.Packet.Deserialization
{
    public class CharacterPacketDeserializationTests
    {
        private readonly IDeserializer _deserializer;

        public CharacterPacketDeserializationTests()
        {
            _deserializer = PacketDeserializationHelper.CreateDeserializer();
        }
        
        [Fact]
        public void CMap_Packet()
        {
            CMapPacket packet = _deserializer.Deserialize<CMapPacket>("c_map 1 20001 1");

            Check.That(packet.MapId).IsEqualTo(20001);
            Check.That(packet.IsBaseMap).IsTrue();
        }
        
        [Fact]
        public void At_Packet()
        {
            AtPacket packet = _deserializer.Deserialize<AtPacket>("at 1234567 2 140 148 2 0 1 1 -1");

            Check.That(packet.CharacterId).Is(1234567);
            Check.That(packet.MapId).Is<short>(2);
            Check.That(packet.PositionX).Is<short>(140);
            Check.That(packet.PositionY).Is<short>(148);
        }

        [Fact]
        public void CInfo_Packet()
        {
            CInfoPacket packet = _deserializer.Deserialize<CInfoPacket>("c_info Mermoud - -1 2858 ~Happy~(Membre) 1234567 0 0 1 9 2 14 0 0 0 0 0 0 0");

            Check.That(packet.Name).Is("Mermoud");
            Check.That(packet.CharacterId).Is(1234567);
            Check.That(packet.Class).Is(ClassType.ARCHER);
            Check.That(packet.Gender).Is(GenderType.MALE);
        }

        [Fact]
        public void Faction_Packet()
        {
            FsPacket packet = _deserializer.Deserialize<FsPacket>("fs 2");

            Check.That(packet.Faction).Is(FactionType.DEMON);
        }

        [Fact]
        public void Fd_Packet()
        {
            FdPacket packet = _deserializer.Deserialize<FdPacket>("fd 49698 14 100 1");

            Check.That(packet.Reputation).Is(49698);
            Check.That(packet.Dignity).Is(100);
        }

        [Fact]
        public void Gold_Packet()
        {
            GoldPacket packet = _deserializer.Deserialize<GoldPacket>("gold 1932933 0");

            Check.That(packet.Gold).Is(1932933);
        }

        [Fact]
        public void Lev_Packet()
        {
            LevPacket packet = _deserializer.Deserialize<LevPacket>("lev 50 3490279 45 122055 11469080 265000 49698 71 0 0 3 0");

            Check.That(packet.Level).Is<byte>(50);
            Check.That(packet.JobLevel).Is<byte>(45);
        }

        [Fact]
        public void Ncif_Packet()
        {
            NcifPacket packet = _deserializer.Deserialize<NcifPacket>("ncif 3 2304");

            Check.That(packet.EntityType).Is(EntityType.MONSTER);
            Check.That(packet.EntityId).Is(2304);
        }

        [Fact]
        public void Pairy_Packet()
        {
            PairyPacket packet = _deserializer.Deserialize<PairyPacket>("pairy 1 1234567 4 2 28 0");

            Check.That(packet.EntityType).Is(EntityType.PLAYER);
            Check.That(packet.EntityId).Is(1234567);
            Check.That(packet.Element).Is(ElementType.WATER);
            Check.That(packet.Power).Is<short>(28);
        }

        [Fact]
        public void Ski_Packet()
        {
            SkiPacket packet = _deserializer.Deserialize<SkiPacket>("ski 833 833 833 834 835 836 837 838 839 840 841 21 25 37 45 353 356");

            Check.That(packet.Skills).ContainsExactly(833, 833, 833, 834, 835, 836, 837, 838, 839, 840, 841, 21, 25, 37, 45, 353, 356);
        }

        [Fact]
        public void Sp_Packet()
        {
            SpPacket packet = _deserializer.Deserialize<SpPacket>("sp 115488 1000000 10000 10000");

            Check.That(packet.Points).Is(10000);
            Check.That(packet.AdditionalPoints).Is(115488);
            Check.That(packet.MaximumPoints).Is(10000);
            Check.That(packet.MaximumAdditionalPoints).Is(1000000);
        }

        [Fact]
        public void Stat_Packet()
        {
            StatPacket packet = _deserializer.Deserialize<StatPacket>("stat 3300 3395 1340 1350 0 416");

            Check.That(packet.Hp).Is(3300);
            Check.That(packet.MaxHp).Is(3395);
            Check.That(packet.Mp).Is(1340);
            Check.That(packet.MaxMp).Is(1350);
        }

        [Fact]
        public void Walk_Packet()
        {
            WalkPacket packet = _deserializer.Deserialize<WalkPacket>("walk 140 87 1 12");

            Check.That(packet.PositionX).Is<short>(140);
            Check.That(packet.PositionY).Is<short>(87);
            Check.That(packet.Speed).Is<byte>(12);
        }
    }
}