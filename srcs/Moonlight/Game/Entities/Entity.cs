using System;
using Moonlight.Core;
using Moonlight.Core.Enums;
using Moonlight.Game.Maps;
using PropertyChanged;

namespace Moonlight.Game.Entities
{
    /// <summary>
    ///     Represent any kind of Entity existing in the game.
    ///     It can be a Player a Monster a Drop or a Npc
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public abstract class Entity : IEquatable<Entity>
    {
        protected Entity(long id, string name, EntityType entityType)
        {
            Id = id;
            Name = name;
            EntityType = entityType;
        }

        /// <summary>
        ///     Id of the entity
        /// </summary>
        public long Id { get; }

        /// <summary>
        ///     Name of the entity
        ///     If entity is a player it will be player name
        ///     If entity is a monster or npc it will be monster/npc name
        ///     If entity is a drop it will be item name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Type of entity
        /// </summary>
        public EntityType EntityType { get; }

        /// <summary>
        ///     Current map where entity is located
        /// </summary>
        public Map Map { get; internal set; }

        /// <summary>
        ///     Current position in Map
        /// </summary>
        public Position Position { get; internal set; }

        /// <summary>
        ///     Check if an Entity is equals to other Entity
        /// </summary>
        /// <param name="other">Other entity</param>
        /// <returns>true if equals false if not</returns>
        public bool Equals(Entity other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id && EntityType == other.EntityType;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Id.GetHashCode() * 397 ^ (int)EntityType;
            }
        }
    }
}