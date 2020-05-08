using System.Linq;
using Moonlight.Clients;
using Moonlight.Core.Logging;
using Moonlight.Event;
using Moonlight.Event.Entities;
using Moonlight.Game.Battle;
using Moonlight.Game.Entities;
using Moonlight.Game.Maps;
using Moonlight.Packet.Battle;

namespace Moonlight.Handlers.Battle
{
    internal class SuPacketHandler : PacketHandler<SuPacket>
    {
        private readonly IEventManager _eventManager;
        private readonly ILogger _logger;

        public SuPacketHandler(ILogger logger, IEventManager eventManager)
        {
            _logger = logger;
            _eventManager = eventManager;
        }

        protected override void Handle(Client client, SuPacket packet)
        {
            if (client.Character == null || client.Character.Map == null)
            {
                return;
            }

            Map map = client.Character.Map;

            LivingEntity caster = map.GetEntity<LivingEntity>(packet.EntityType, packet.EntityId);
            LivingEntity target = map.GetEntity<LivingEntity>(packet.TargetEntityType, packet.TargetEntityId);

            if (caster is Character character)
            {
                if (character.Skills != null)
                {
                    Skill skill = character.Skills.FirstOrDefault(x => x.Id == packet.SkillVnum);
                    if (skill != null)
                    {
                        skill.IsOnCooldown = true;
                    }
                }
            }

            if (target == null || caster == null)
            {
                return;
            }

            target.HpPercentage = packet.TargetHpPercentage;

            _eventManager.Emit(new EntityDamageEvent(client)
            {
                Entity = target,
                Attacker = caster,
                Damage = packet.Damage
            });

            if (packet.TargetIsAlive)
            {
                return;
            }

            target.HpPercentage = 0;

            map.RemoveEntity(target);
            _logger.Info($"Entity {target.EntityType} {target.Id} died");

            _eventManager.Emit(new EntityDeathEvent(client)
            {
                Entity = target,
                Killer = caster
            });
        }
    }
}