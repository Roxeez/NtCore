using System;
using System.Linq;
using System.Threading.Tasks;
using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Core.Collection;
using Moonlight.Core.Enums;
using Moonlight.Core.Logging;
using Moonlight.Game.Battle;
using Moonlight.Game.Inventories;
using Moonlight.Game.Maps;
using Moonlight.Game.Raids;

namespace Moonlight.Game.Entities
{
    /// <summary>
    ///     Represent the Character of a session
    ///     A character can be controlled not like a Player entity
    /// </summary>
    public class Character : Player
    {
        private readonly ILogger _logger;
        
        internal Character(ILogger logger, long id, string name, Client client) : base(id, name)
        {
            _logger = logger;

            Client = client;
            Inventory = new Inventory(this);
            Skills = new InternalObservableHashSet<Skill>();
            Unsafe = new Unsafe(this);
        }

        /// <summary>
        ///     Client used for updating values of this Character
        /// </summary>
        public Client Client { get; }

        /// <summary>
        ///     Player job level
        /// </summary>
        public byte JobLevel { get; internal set; }

        /// <summary>
        ///     Current player Mp
        /// </summary>
        public int Hp { get; internal set; }

        /// <summary>
        ///     Maximum player Mp
        /// </summary>
        public int MaxHp { get; internal set; }

        /// <summary>
        ///     Current player Mp
        /// </summary>
        public int Mp { get; internal set; }

        /// <summary>
        ///     Maximum player Mp
        /// </summary>
        public int MaxMp { get; internal set; }

        /// <summary>
        ///     Represent character global inventory (gold, bags etc...)
        /// </summary>
        public Inventory Inventory { get; }

        /// <summary>
        ///     Current character gold
        /// </summary>
        public int Gold { get; set; }

        /// <summary>
        ///     Current sp points
        /// </summary>
        public int SpPoints { get; internal set; }

        /// <summary>
        ///     Current sp points
        /// </summary>
        public int AdditionalSpPoints { get; internal set; }

        /// <summary>
        ///     Current character production points
        /// </summary>
        public short ProductionPoints { get; internal set; }

        /// <summary>
        ///     Current character skills
        /// </summary>
        public InternalObservableHashSet<Skill> Skills { get; }

        /// <summary>
        ///     Class containing unsafe code (game exploit) (use it at your own risk)
        /// </summary>
        public Unsafe Unsafe { get; }

        /// <summary>
        ///     Raid the player is currently in
        /// </summary>
        public Raid Raid { get; internal set; }

        internal DateTime LastMovement { get; set; }

        public override byte HpPercentage => (byte)(Hp == 0 ? 0 : (double)Hp / MaxHp * 100);
        public override byte MpPercentage => (byte)(Mp == 0 ? 0 : (double)Mp / MaxMp * 100);

        /// <summary>
        ///     Walk to the specified position
        /// </summary>
        /// <param name="position">Position where you want to walk</param>
        public async Task Walk(Position position)
        {
            if (Position.Equals(position))
            {
                _logger.Info("Walk cancelled : already at target position");
                return;
            }

            if (Client is LocalClient local)
            {
                local.ManagedMoonlightCore.Walk(position.X, position.Y);
            }

            LastMovement = DateTime.Now;

            while (LastMovement.AddMilliseconds(1100) > DateTime.Now)
            {
                await Task.Delay(100).ConfigureAwait(false);
            }
        }

        /// <summary>
        ///     Walk until character in range of the specific position
        /// </summary>
        /// <param name="position">Position where you want to walk</param>
        /// <param name="range">Range wanted</param>
        public async Task WalkInRange(Position position, int range)
        {
            int distance = Position.GetDistance(position);
            if (distance <= range)
            {
                _logger.Info("Walk cancelled : already in range");
                return;
            }

            double distRatio = (distance - range) / (double)distance;
            double x = Position.X + distRatio * (position.X - Position.X);
            double y = Position.Y + distRatio * (position.Y - Position.Y);

            await Walk(new Position((short)x, (short)y)).ConfigureAwait(false);
        }

        /// <summary>
        ///     Attack entity with basic attack
        /// </summary>
        /// <param name="entity">Entity to attack</param>
        public async Task Attack(LivingEntity entity)
        {
            if (entity.Equals(this))
            {
                _logger.Info("Attack cancelled : Can't attack yourself");
                return;
            }

            Skill skill = Skills.FirstOrDefault();
            if (skill == null)
            {
                _logger.Info("Attack cancelled : No base skill");
                return;
            }

            await UseSkillOn(skill, entity).ConfigureAwait(false);
        }

        /// <summary>
        ///     Use a skill on yourself
        ///     Skill wont be used if
        ///     - Skill is not in your skills
        ///     - Skill is on cooldown
        ///     - If this skill can't target you
        /// </summary>
        /// <param name="skill">Skill to use</param>
        public async Task UseSkill(Skill skill)
        {
            if (!Skills.Contains(skill))
            {
                _logger.Info("Attack cancelled : Skill is not in skill list");
                return;
            }

            if (skill.IsOnCooldown)
            {
                return;
            }

            if (skill.TargetType == TargetType.TARGET)
            {
                return;
            }

            Client.SendPacket($"u_s {skill.CastId} {(int)EntityType} {Id}");
            await Task.Delay(skill.CastTime * 200).ConfigureAwait(false);
        }

        /// <summary>
        ///     Use a skill on target (walk to target if not in range)
        ///     Skill wont be used if
        ///     - Skill is not in your skills
        ///     - Skill is on cooldown
        ///     - If this skill has no target
        /// </summary>
        /// <param name="skill">Skill to use</param>
        /// <param name="target">Target to hit with skill</param>
        public async Task UseSkillOn(Skill skill, LivingEntity target)
        {
            if (!Skills.Contains(skill))
            {
                _logger.Info("Attack cancelled : Skill is not in skill list");
                return;
            }

            if (skill.IsOnCooldown)
            {
                return;
            }

            if (skill.TargetType == TargetType.SELF)
            {
                await UseSkill(skill).ConfigureAwait(false);
                return;
            }

            if (target.Equals(this))
            {
                return;
            }

            if (skill.TargetType == TargetType.NO_TARGET)
            {
                return;
            }

            await WalkInRange(target.Position, skill.Range).ConfigureAwait(false);
            Client.SendPacket($"u_s {skill.CastId} {(int)target.EntityType} {target.Id}");
            await Task.Delay(skill.CastTime * 200).ConfigureAwait(false);
        }

        /// <summary>
        ///     Use a skill at position (walk to position range if not in range)
        ///     Skill wont be used if
        ///     - Skill is not in your skills
        ///     - Skill is on cooldown
        ///     - Skill need a target
        /// </summary>
        /// <param name="skill">Skill to use</param>
        /// <param name="position">Position where you want to use the skill</param>
        public async Task UseSkillAt(Skill skill, Position position)
        {
            if (!Skills.Contains(skill))
            {
                return;
            }

            if (skill.IsOnCooldown)
            {
                return;
            }

            if (skill.TargetType != TargetType.NO_TARGET)
            {
                return;
            }

            await WalkInRange(position, skill.Range).ConfigureAwait(false);
            Client.SendPacket($"u_as {skill.CastId} {position.X} {position.Y}");
            await Task.Delay(skill.CastTime * 100).ConfigureAwait(false);
        }

        /// <summary>
        ///     Pickup a ground item
        ///     If item is lever it will start activating the lever
        ///     If it's a drop it will just pickup it
        /// </summary>
        /// <param name="groundItem">Object to pick</param>
        public async Task PickUp(GroundItem groundItem)
        {
            await WalkInRange(groundItem.Position, 1).ConfigureAwait(false);
            Client.SendPacket($"get {(byte)EntityType} {Id} {groundItem.Id}");
        }
    }
}