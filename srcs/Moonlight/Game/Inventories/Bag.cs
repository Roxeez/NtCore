using System.Collections.Generic;
using System.Linq;
using Moonlight.Core.Collection;
using Moonlight.Core.Enums;
using Moonlight.Game.Entities;
using Moonlight.Game.Inventories.Items;

namespace Moonlight.Game.Inventories
{
    public class Bag : InternalObservableDictionary<int, ItemInstance>
    {
        private readonly Character _character;

        public Bag(Character character, BagType bagType)
        {
            _character = character;
            BagType = bagType;
        }

        public BagType BagType { get; }

        public void Move(int sourceSlot, int destinationSlot)
        {
            ItemInstance item = GetValueOrDefault(sourceSlot);
            if (item == null)
            {
                return;
            }

            if (destinationSlot == sourceSlot)
            {
                return;
            }

            _character.Client.SendPacket($"mvi {BagType} {sourceSlot} {item.Amount} {destinationSlot}");
        }

        public void Move(int sourceSlot, int destinationSlot, int amount)
        {
            ItemInstance item = GetValueOrDefault(sourceSlot);
            if (item == null)
            {
                return;
            }

            if (destinationSlot == sourceSlot)
            {
                return;
            }

            if (amount > item.Amount || amount < 1)
            {
                return;
            }

            _character.Client.SendPacket($"mvi {BagType} {sourceSlot} {amount} {destinationSlot}");
        }

        public void Use(Item item)
        {
            KeyValuePair<int, ItemInstance> entry = ThreadSafeInternalDictionary.FirstOrDefault(x => x.Value.Item.Equals(item));
            if (entry.Equals(default))
            {
                return;
            }

            Use(entry.Key);
        }

        public void Use(int slot)
        {
            if (!ContainsKey(slot))
            {
                return;
            }

            _character.Client.SendPacket($"u_i {(int)_character.EntityType} {_character.Id} {(int)BagType} {slot} 0 0 ");
        }

        public void Drop(int slot, int amount)
        {
            ItemInstance item = GetValueOrDefault(slot);
            if (item == null)
            {
                return;
            }

            if (amount <= 0)
            {
                return;
            }

            if (amount > item.Amount || amount < 1)
            {
                return;
            }

            _character.Client.SendPacket($"put {(int)BagType} {slot} {amount}");
        }

        public void Drop(int slot)
        {
            ItemInstance item = GetValueOrDefault(slot);
            if (item == null)
            {
                return;
            }

            _character.Client.SendPacket($"put {(int)BagType} {slot} {item.Amount}");
        }
    }
}