using Moonlight.Game.Battle;
using Moonlight.Game.Entities;
using Moonlight.Game.Maps;
using Moonlight.Tests.Extensions;
using NFluent;
using Xunit;

namespace Moonlight.Tests.Packet.Handling
{
    public class BattlePacketHandlingTests : PacketHandlingTests
    {
        [Fact]
        public void Sr_Packet_Reset_Skill_Cooldown()
        {
            Skill skill = SkillFactory.CreateSkill(450);
            Character.Skills.Add(skill);
            
            skill.IsOnCooldown = true;
            Client.ReceivePacket($"sr {skill.CastId}");
            
            Check.That(skill.IsOnCooldown).IsFalse();
        }

        [Fact]
        public void Su_Packet_Damage_Target()
        {
            Map map = Character.Map;
            Monster monster = EntityFactory.CreateMonster(2077, 1);

            monster.HpPercentage = 100;
            map.AddEntity(monster);
            
            Client.ReceivePacket("su 1 999 3 2077 240 8 11 257 0 0 1 80 721 0 0");

            Check.That(monster.HpPercentage).IsEqualTo(80);
        }

        [Fact]
        public void Su_Packet_Kill_Target()
        {
            Map map = Character.Map;
            Monster monster = EntityFactory.CreateMonster(2077, 1);
            
            map.AddEntity(monster);
            
            Client.ReceivePacket("su 1 999 3 2077 240 8 11 257 0 0 0 0 1721 0 0");

            Check.That(monster.HpPercentage).IsEqualTo(0);
            Check.That(map.Monsters).Not.HasElementThatMatches(x => x.Id == 2077);
        }

        [Fact]
        public void Su_Packet_Set_Character_Skill_On_Cooldown()
        {
            Skill skill = SkillFactory.CreateSkill(254);
            
            Character.Skills.Add(skill);
            
            Client.ReceivePacket("su 1 999 1 999 254 333 24 281 0 0 1 100 0 -1 0");

            Check.That(skill.IsOnCooldown).IsTrue();
        }
    }
}