﻿using NtCore.API.Game.Battle;
using NtCore.API.Game.Entities;
using NtCore.API.Plugins;

namespace NtCore.API.Events.Character
{
    public class TargetStatUpdateEvent : Event
    {
        public TargetStatUpdateEvent(ICharacter character, ITarget target)
        {
            Character = character;
            Target = target;
        }

        public ICharacter Character { get; }
        public ITarget Target { get; }
    }
}