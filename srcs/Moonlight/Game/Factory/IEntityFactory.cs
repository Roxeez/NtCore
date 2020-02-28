using Moonlight.Game.Entities;

namespace Moonlight.Game.Factory
{
    public interface IEntityFactory
    {
        Player CreatePlayer(long id, string name);
        Monster CreateMonster(long id, int vnum);
        Npc CreateNpc(long id, int vnum, string name);
        GroundItem CreateGroundItem(long id, int vnum, int amount);
    }
}