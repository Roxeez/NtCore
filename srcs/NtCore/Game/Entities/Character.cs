using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NtCore.Clients;
using NtCore.Core;
using NtCore.Enums;
using NtCore.Extensions;
using NtCore.Game.Battle;
using NtCore.Game.Data;
using NtCore.Game.Inventories;
using NtCore.Game.Relation;
using NtCore.Import;

namespace NtCore.Game.Entities
{
    /// <summary>
    /// Represent your character in the game
    /// </summary>
    public class Character : Player
    {
        public Character(IClient client)
        {
            Client = client;
            LastMapChange = DateTime.Now;
            Equipment = new Equipment();
            Skills = new HashSet<Skill>();
            Friends = new List<Friend>();
            SpPointInfo = new SpPointInfo();
        }

        /// <summary>
        /// Client bind to this character
        /// </summary>
        [NotNull]
        public IClient Client { get; }

        /// <summary>
        /// Character wear equipment
        /// </summary>
        [NotNull]
        public Equipment Equipment { get; internal set; }

        /// <summary>
        /// Character target information
        /// </summary>
        [CanBeNull]
        public Target Target { get; internal set; }

        /// <summary>
        /// Hp percentage of character
        /// </summary>
        public new byte HpPercentage => (byte)(Hp == 0 ? 0 : (double)Hp / MaxHp * 100);

        /// <summary>
        /// Mp percentage of character
        /// </summary>
        public new byte MpPercentage => (byte)(Mp == 0 ? 0 : (double)Mp / MaxMp * 100);

        /// <summary>
        /// Character current hp
        /// </summary>
        public int Hp { get; internal set; }

        /// <summary>
        /// Character max hp
        /// </summary>
        public int MaxHp { get; internal set; }

        /// <summary>
        /// Character current mp
        /// </summary>
        public int Mp { get; internal set; }

        /// <summary>
        /// Character max mp
        /// </summary>
        public int MaxMp { get; internal set; }

        /// <summary>
        /// Current character party
        /// </summary>
        [CanBeNull]
        public Party Party { get; internal set; }

        /// <summary>
        /// Current job level of character
        /// </summary>
        public byte JobLevel { get; internal set; }

        /// <summary>
        /// Contains all informations about character sp points
        /// </summary>
        [NotNull]
        public SpPointInfo SpPointInfo { get; }

        /// <summary>
        /// Current gold of character
        /// </summary>
        public int Gold { get; internal set; }

        /// <summary>
        /// Last character map change
        /// </summary>
        public DateTime LastMapChange { get; internal set; }

        /// <summary>
        /// Contains all skills of character
        /// </summary>
        [NotNull]
        public HashSet<Skill> Skills { get; }

        /// <summary>
        /// Contains all friends of character
        /// </summary>
        [NotNull]
        public IEnumerable<Friend> Friends { get; internal set; }

        /// <summary>
        /// Use a skill on yourself
        /// </summary>
        /// <param name="skill">Skill to use</param>
        /// <returns></returns>
        public async Task UseSkill([NotNull] Skill skill)
        {
            if (!Skills.Contains(skill))
            {
                return;
            }

            if (skill.Info.TargetingType != TargetingType.SELF && skill.Info.TargetingType != TargetingType.SELF_OR_TARGET)
            {
                return;
            }

            await Client.SendPacket($"u_s {skill.Info.CastId} {(byte)EntityType} {Id}");
        }

        /// <summary>
        /// Use skill on entity
        /// </summary>
        /// <param name="skill">Skill to use</param>
        /// <param name="target">Target of the skill</param>
        /// <returns></returns>
        public async Task UseSkillOn([NotNull] Skill skill, [NotNull] LivingEntity target)
        {
            if (!Skills.Contains(skill))
            {
                return;
            }

            if (skill.Info.TargetingType == TargetingType.SELF)
            {
                await UseSkill(skill);
                return;
            }

            if (!Position.IsInRange(target.Position, skill.Info.Range))
            {
                await WalkInRange(target.Position, skill.Info.Range);
            }

            await Client.SendPacket($"u_s {skill.Info.CastId} {(byte)target.EntityType} {target.Id}");
        }

        /// <summary>
        /// Make your character rest
        /// </summary>
        /// <returns></returns>
        public async Task Rest()
        {
            IsResting = !IsResting;

            await Client.SendPacket($"rest 1 1 {Id}");
        }

        /// <summary>
        /// Use skill at selected position
        /// </summary>
        /// <param name="skill">Skill to use</param>
        /// <param name="position">Target position</param>
        /// <returns></returns>
        public async Task UseSkillAt([NotNull] Skill skill, Position position)
        {
            if (!Skills.Contains(skill))
            {
                return;
            }

            if (skill.Info.TargetingType != TargetingType.NO_TARGET)
            {
                return;
            }

            if (!Position.IsInRange(position, skill.Info.Range))
            {
                await WalkInRange(position, skill.Info.Range);
            }

            await Client.SendPacket($"u_as {skill.Info.CastId} {position.X} {position.Y}");
        }

        /// <summary>
        /// Make your character pick up a drop
        /// </summary>
        /// <param name="drop">Drop to pickup</param>
        /// <returns></returns>
        public async Task PickUp([NotNull] Drop drop)
        {
            if (drop.Owner != null && !drop.Owner.Equals(this))
            {
                return;
            }

            if (!drop.Position.IsInArea(Position, 2))
            {
                return;
            }

            await Client.SendPacket($"get {(byte)EntityType} {Id} {drop.Id}");
        }

        /// <summary>
        /// Walk to selected position
        /// </summary>
        /// <param name="destination">Destination position</param>
        /// <returns></returns>
        public async Task Walk(Position destination)
        {
            if (!Map.IsWalkable(destination))
            {
                return;
            }

            bool positiveX = destination.X > Position.X;
            bool positiveY = destination.Y > Position.Y;

            while (!Position.Equals(destination))
            {
                var position = new Position(Position.X, Position.Y);

                int distanceX = position.GetDistanceX(destination);
                int distanceY = position.GetDistanceY(destination);

                int stepX = distanceX >= 5 ? 5 : distanceX;
                int stepY = distanceY >= 5 ? 5 : distanceY;

                position.X = (short)((positiveX ? 1 : -1) * stepX + position.X);
                position.Y = (short)((positiveY ? 1 : -1) * stepY + position.Y);

                if (!Map.IsWalkable(position))
                {
                    return;
                }

                if (Client.IsLocal())
                {
                    NtNative.Walk(position.X, position.Y);
                }
                else
                {
                    await Client.SendPacket($"walk {position.X} {position.Y} {(position.X + position.Y) % 3 % 2} {Speed}");
                }

                int time = (stepX + stepY) * (2000 / Speed);
                await Task.Delay(time);

                Position = position;
            }
        }
        
        /// <summary>
        /// Walk in range of position
        /// </summary>
        /// <param name="destination">Destination position</param>
        /// <param name="range">Range</param>
        /// <returns></returns>
        public async Task WalkInRange(Position destination, int range)
        {
            int distance = Position.GetDistance(destination);
            if (distance <= range)
            {
                return;
            }

            double distRatio = (distance - range) / (double)distance;
            double x = Position.X + distRatio * (destination.X - Position.X);
            double y = Position.Y + distRatio * (destination.Y - Position.Y);

            await Walk(new Position((short)x, (short)y));
        }

        /// <summary>
        /// Target selected entity
        /// </summary>
        /// <param name="entity">Entity to target, if null remove current target</param>
        /// <returns></returns>
        public async Task TargetEntity([CanBeNull] LivingEntity entity)
        {
            if (entity == null)
            {
                Target = null;
                return;
            }

            await Client.SendPacket($"ncif {(byte)entity.EntityType} {entity.Id}");
        }

        /// <summary>
        /// Send a friend request to selected player
        /// </summary>
        /// <param name="player">Receiver of the friend request</param>
        /// <returns></returns>
        public async Task SendFriendRequest([NotNull] Player player)
        {
            await Client.SendPacket($"fins {(byte)player.EntityType} {player.Id}");
        }

        /// <summary>
        /// Fake a received dialog
        /// </summary>
        /// <param name="message">Message to display in dialog</param>
        /// <returns></returns>
        public async Task ShowInfoDialog([NotNull] string message)
        {
            await Client.ReceivePacket($"info {message}");
        }

        /// <summary>
        /// Fake a received server message
        /// </summary>
        /// <param name="message">Message to receive</param>
        /// <param name="messageType">Type of message</param>
        /// <returns></returns>
        public async Task ShowMessage([NotNull] string message, MessageType messageType)
        {
            await Client.ReceivePacket($"msg {(byte)messageType} {message}");
        }

        /// <summary>
        /// Fake a received chat message
        /// </summary>
        /// <param name="message">Message to receive</param>
        /// <param name="messageColor">Chat message color</param>
        /// <returns></returns>
        public async Task ShowChatMessage([NotNull] string message, ChatMessageColor messageColor)
        {
            await Client.ReceivePacket($"say {(byte)EntityType} {Id} {(byte)messageColor} {message}");
        }

        /// <summary>
        /// Show a bubble message on character
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <returns></returns>
        public async Task ShowBubbleMessage([NotNull] string message)
        {
            await Client.ReceivePacket($"say {(byte)EntityType} {Id} 1 {message}");
        }

        /// <summary>
        /// Show a bubble message on a living entity
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="entity">Entity where bubble will be</param>
        /// <returns></returns>
        public async Task ShowBubbleMessageOn([NotNull] string message, [NotNull] LivingEntity entity)
        {
            await Client.ReceivePacket($"say {(byte)entity.EntityType} {entity.Id} 1 {message}");
        }
    }
}