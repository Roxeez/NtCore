using System;
using Moonlight.Core.Enums;
using Moonlight.Database.DAL;
using Moonlight.Database.Dto;
using Moonlight.Game.Entities;
using Moonlight.Game.Inventories.Items;
using Moonlight.Translation;

namespace Moonlight.Game.Factory.Impl
{
    internal class EntityFactory : IEntityFactory
    {
        private readonly IItemFactory _itemFactory;
        private readonly ILanguageService _languageService;
        private readonly IRepository<MonsterDto> _monsterRepository;

        public EntityFactory(ILanguageService languageService, IRepository<MonsterDto> monsterRepository, IItemFactory itemFactory)
        {
            _languageService = languageService;
            _monsterRepository = monsterRepository;
            _itemFactory = itemFactory;
        }

        public Player CreatePlayer(long id, string name) => new Player(id, name);

        public Monster CreateMonster(long id, int vnum)
        {
            MonsterDto monsterDto = _monsterRepository.Select(vnum);
            if (monsterDto == null)
            {
                throw new InvalidOperationException($"Can't find monster {vnum} in database");
            }

            string name = _languageService.GetTranslation(RootKey.MONSTER, monsterDto.NameKey);
            return new Monster(id, monsterDto, name);
        }

        public Npc CreateNpc(long id, int vnum, string name)
        {
            MonsterDto monsterDto = _monsterRepository.Select(vnum);
            if (monsterDto == null)
            {
                throw new InvalidOperationException($"Can't find monster {vnum} in database");
            }

            if (name == null || name == "-")
            {
                name = _languageService.GetTranslation(RootKey.MONSTER, monsterDto.NameKey);
            }

            return new Npc(id, monsterDto, name);
        }

        public GroundItem CreateGroundItem(long id, int vnum, int amount)
        {
            Item item = _itemFactory.CreateItem(vnum);
            return new GroundItem(id, item, amount);
        }
    }
}