﻿using Moonlight.Core;
using Moonlight.Game.Entities;

namespace Moonlight.Event.Entities
{
    public class EntityMoveEvent : IEventNotification
    {
        public LivingEntity Entity { get; internal set; }
        public Position From { get; internal set; }
        public Position To { get; internal set; }
    }
}