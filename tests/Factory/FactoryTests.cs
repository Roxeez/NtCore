using Moonlight.Core;
using Moonlight.Core.Enums;
using Moonlight.Database;
using Moonlight.Database.DAL;
using Moonlight.Database.Dto;
using Moonlight.Database.Entities;
using Moonlight.Game.Factory;
using Moonlight.Game.Factory.Impl;
using Moonlight.Game.Maps;
using Moonlight.Tests.Extensions;
using Moonlight.Translation;
using NFluent;
using Xunit;
using Map = Moonlight.Database.Entities.Map;

namespace Moonlight.Tests.Factory
{
    public class FactoryTests
    {
        public FactoryTests()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddLogger();
            services.AddPacketDependencies();
            services.AddDatabaseDependencies(new AppConfig());
            services.AddFactories();
            services.AddSingleton<ILanguageService, LanguageService>();

            ServiceProvider provider = services.BuildServiceProvider();

            _itemFactory = provider.GetService<IItemFactory>();
            _entityFactory = provider.GetService<IEntityFactory>();
            _mapFactory = provider.GetService<IMapFactory>();
            _minilandObjectFactory = provider.GetService<IMinilandObjectFactory>();
            _languageService = provider.GetService<ILanguageService>();
        }

        private readonly IItemFactory _itemFactory;
        private readonly IEntityFactory _entityFactory;
        private readonly IMapFactory _mapFactory;
        private readonly IMinilandObjectFactory _minilandObjectFactory;
        private readonly ILanguageService _languageService;

        [Fact]
        public void Entity_Factory_Create_Correct_Entity()
        {
            Game.Entities.Monster monster = _entityFactory.CreateMonster(1, 125);

            Check.That(monster).IsNotNull();
            Check.That(monster.Id).IsEqualTo(1);
            Check.That(monster.Vnum).IsEqualTo(125);
        }

        [Fact]
        public void Item_Factory_Create_Correct_Item()
        {
            Game.Inventories.Items.Item item = _itemFactory.CreateItem(3125);

            Check.That(item).IsNotNull();
            Check.That(item.Vnum).IsEqualTo(3125);
        }

        [Fact]
        public void Language_Service_Return_Correct_Value()
        {
            string value = _languageService.GetTranslation(RootKey.SKILL, "zts174e");

            Check.That(value).Is("Rain of Arrows");
        }

        [Fact]
        public void Map_Factory_Create_Correct_Map()
        {
            Game.Maps.Map map = _mapFactory.CreateMap(1);

            Check.That(map.Id).IsEqualTo(1);
            Check.That(map.Name).IsEqualTo("NosVille");
        }

        [Fact]
        public void Miniland_Object_Factory_Create_Correct_Object()
        {
            MinilandObject minilandObject = _minilandObjectFactory.CreateMinilandObject(3125, 1, new Position(5, 5));

            Check.That(minilandObject.Item.Vnum).IsEqualTo(3125);
            Check.That(minilandObject.Slot).IsEqualTo(1);
            Check.That(minilandObject.Position).IsEqualTo(new Position(5, 5));
        }
    }
}