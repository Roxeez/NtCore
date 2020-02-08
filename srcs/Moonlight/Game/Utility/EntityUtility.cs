﻿using System;
using System.Collections.Generic;
using Moonlight.Core.Enums;
using Moonlight.Core.Extensions;
using Moonlight.Game.Entities;

namespace Moonlight.Game.Utility
{
    public static class EntityUtility
    {
        private static readonly Dictionary<Type, EntityType> _typeMapping;

        static EntityUtility() =>
            _typeMapping = new Dictionary<Type, EntityType>
            {
                [typeof(Monster)] = EntityType.MONSTER,
                [typeof(Npc)] = EntityType.NPC,
                [typeof(Player)] = EntityType.PLAYER,
                [typeof(Drop)] = EntityType.DROP
            };

        public static EntityType GetEntityType<T>() where T : Entity => _typeMapping.GetValueOrDefault(typeof(T));
    }
}