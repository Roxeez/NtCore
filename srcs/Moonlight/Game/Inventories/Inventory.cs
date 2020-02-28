using System.Collections.Generic;
using Moonlight.Core.Enums;
using Moonlight.Extensions;
using Moonlight.Game.Entities;

namespace Moonlight.Game.Inventories
{
    public class Inventory
    {
        private readonly Dictionary<BagType, Bag> _bags;

        public Inventory(Character character)
        {
            Equipment = new Bag(character, BagType.EQUIPMENT);
            Main = new Bag(character, BagType.MAIN);
            Etc = new Bag(character, BagType.ETC);
            Miniland = new Bag(character, BagType.MINILAND);
            Specialist = new Bag(character, BagType.SPECIALIST);
            Costume = new Bag(character, BagType.COSTUME);

            _bags = new Dictionary<BagType, Bag>
            {
                [Equipment.BagType] = Equipment,
                [Main.BagType] = Main,
                [Etc.BagType] = Etc,
                [Miniland.BagType] = Miniland,
                [Specialist.BagType] = Specialist,
                [Costume.BagType] = Costume
            };
        }

        public Bag Equipment { get; }
        public Bag Main { get; }
        public Bag Etc { get; }
        public Bag Miniland { get; }
        public Bag Specialist { get; }
        public Bag Costume { get; }

        public Bag GetBag(BagType bagType) => _bags.GetValueOrDefault(bagType);
    }
}