using System;
using System.Linq;
using Moonlight.Core.Enums;
using Moonlight.Database.Dto;

namespace Moonlight.Game.Inventories.Items
{
    public class Item : IEquatable<Item>
    {
        private readonly ItemDto _itemDto;

        internal Item(string name, ItemDto itemDto)
        {
            _itemDto = itemDto;
            
            Name = name;
            Data = _itemDto.Data?.Split('|').Select(x => Convert.ToInt16(x)).ToArray() ?? Array.Empty<short>();
        }

        public int Vnum => _itemDto.Id;
        public string Name { get; }

        public int Type => _itemDto.Type;
        public int SubType => _itemDto.SubType;
        public BagType BagType => _itemDto.BagType;
        public short[] Data { get; }
    
        public bool Equals(Item other) => other != null && other.Vnum == Vnum;
    }
}