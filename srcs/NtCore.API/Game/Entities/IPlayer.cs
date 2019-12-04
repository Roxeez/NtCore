﻿using NtCore.API.Enums;

namespace NtCore.API.Game.Entities
{
    public interface IPlayer : ILivingEntity
    {
        string Name { get; }
        int Reputation { get; }
        int Dignity { get; }
        Faction Faction { get; }
    }
}