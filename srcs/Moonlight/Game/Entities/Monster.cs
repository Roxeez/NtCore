using Moonlight.Core.Enums;
using Moonlight.Database.Dto;

namespace Moonlight.Game.Entities
{
    /// <summary>
    ///     Represent any kind of Monster
    /// </summary>
    public class Monster : LivingEntity
    {
        public int Vnum { get; internal set; }

        public bool IsRaidBoss { get; internal set; }
        
        internal Monster(long id, string name) : base(id, name, EntityType.MONSTER)
        {
            
        }
    }
}