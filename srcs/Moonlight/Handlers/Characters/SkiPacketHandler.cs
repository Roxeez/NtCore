using System.Collections.Generic;
using Moonlight.Clients;
using Moonlight.Core.Enums;
using Moonlight.Game.Battle;
using Moonlight.Game.Factory;
using Moonlight.Packet.Character;

namespace Moonlight.Handlers.Characters
{
    internal class SkiPacketHandler : PacketHandler<SkiPacket>
    {
        private readonly ISkillFactory _skillFactory;

        public SkiPacketHandler(ISkillFactory skillFactory) => _skillFactory = skillFactory;

        protected override void Handle(Client client, SkiPacket packet)
        {
            if (client.Character == null)
            {
                return;
            }

            client.Character.Skills.Clear();

            var skills = new List<Skill>();
            foreach (int skillVnum in packet.Skills)
            {
                Skill skill = _skillFactory.CreateSkill(skillVnum);
                if (skill.SkillType == SkillType.PLAYER)
                {
                    skills.Add(skill);
                }

                // TODO : Maybe we can add passive etc...
            }

            skills.ForEach(x => client.Character.Skills.Add(x));
        }
    }
}