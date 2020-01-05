﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using NtCore.Core;
using NtCore.Enums;
using NtCore.Game.Battle;
using NtCore.Game.Data;
using NtCore.Game.Inventories;
using NtCore.Game.Relation;

namespace NtCore.Game.Entities
{
    public interface ICharacter : IPlayer
    {
        /// <summary>
        /// Character wear equipment
        /// </summary>
        IEquipment Equipment { get; }

        /// <summary>
        /// Current target (null if none)
        /// </summary>
        ITarget Target { get; }
        
        /// <summary>
        ///     Job level
        /// </summary>
        byte JobLevel { get; }
        
        /// <summary>
        /// Info about current sp points
        /// </summary>
        SpPointInfo SpPointInfo { get; }

        /// <summary>
        ///     Current golds
        /// </summary>
        int Gold { get; }

        /// <summary>
        ///     Last time map changed
        /// </summary>
        DateTime LastMapChange { get; }

        /// <summary>
        ///     Current character hp
        /// </summary>
        int Hp { get; }

        /// <summary>
        ///     Character max hp
        /// </summary>
        int MaxHp { get; }

        /// <summary>
        ///     Current character mp
        /// </summary>
        int Mp { get; }

        /// <summary>
        ///     Character max mp
        /// </summary>
        int MaxMp { get; }

        /// <summary>
        /// Current party
        /// </summary>
        IParty Party { get; }

        /// <summary>
        ///     Contains all character skills
        /// </summary>
        HashSet<ISkill> Skills { get; }

        /// <summary>
        ///     Contains all character friends
        /// </summary>
        IEnumerable<IFriend> Friends { get; }

        /// <summary>
        ///     Use selected skill on self
        /// </summary>
        /// <param name="skill">Skill to use</param>
        Task UseSkill([NotNull] ISkill skill);

        /// <summary>
        ///     Use skill on defined target
        /// </summary>
        /// <param name="skill">Skill to use</param>
        /// <param name="target">Target of the skill</param>
        Task UseSkill([NotNull] ISkill skill, [NotNull] ILivingEntity target);

        /// <summary>
        ///     Use skill at specific position
        /// </summary>
        /// <param name="skill">Skill to use</param>
        /// <param name="position">Target position</param>
        Task UseSkill([NotNull] ISkill skill, Position position);

        /// <summary>
        ///     Get drop on ground
        /// </summary>
        /// <param name="drop">Drop to pick up</param>
        Task PickUp([NotNull] IDrop drop);

        /// <summary>
        ///     Make your character move
        /// </summary>
        /// <param name="position">Position where you want to move</param>
        Task Move(Position position);

        Task SelectEntity([CanBeNull] ILivingEntity entity);

        /// <summary>
        ///     Send a friend request to selected player
        /// </summary>
        /// <param name="player">Selected player</param>
        Task SendFriendRequest([NotNull] IPlayer player);

        /// <summary>
        ///     Show info dialog with selected message
        /// </summary>
        /// <param name="message">Message to show</param>
        Task ShowInfoDialog(string message);

        /// <summary>
        ///     Show a message to character (clientside only it's a received packet)
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="messageType">Type of message</param>
        Task ReceiveMessage([NotNull] string message, MessageType messageType);

        /// <summary>
        ///     Show a chat message to character (clientside only it's a received packet)
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="messageColor">Type of message</param>
        Task ReceiveChatMessage([NotNull] string message, ChatMessageColor messageColor);

        /// <summary>
        ///     Show a bubble message on top of character with selected message
        /// </summary>
        /// <param name="message">Message to show</param>
        Task ShowBubbleMessage([NotNull] string message);

        /// <summary>
        ///     Show a bubble message on top of entity with selected message
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="entity">Entity where bubble need to be</param>
        Task ShowBubbleMessage([NotNull] string message, [NotNull] ILivingEntity entity);
    }
}