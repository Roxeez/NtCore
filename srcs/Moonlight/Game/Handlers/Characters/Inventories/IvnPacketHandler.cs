﻿using Moonlight.Clients;
using Moonlight.Core.Enums;
using Moonlight.Database.Entities;
using Moonlight.Game.Entities;
using Moonlight.Game.Factory;
using Moonlight.Game.Inventories;
using Moonlight.Game.Inventories.Items;
using Moonlight.Packet.Character.Inventory;
using Item = Moonlight.Game.Inventories.Items.Item;

namespace Moonlight.Game.Handlers.Characters.Inventories
{
    internal class IvnPacketHandler : PacketHandler<IvnPacket>
    {
        private readonly IItemInstanceFactory _itemInstanceFactory;

        public IvnPacketHandler(IItemInstanceFactory itemInstanceFactory)
        {
            _itemInstanceFactory = itemInstanceFactory;
        }
        
        protected override void Handle(Client client, IvnPacket packet)
        {
            Character character = client.Character;

            Bag bag = character.Inventory.GetBag(packet.BagType);
            if (bag == null)
            {
                return;
            }

            if (packet.SubPacket.VNum == -1)
            {
                bag.RemoveItemInSlot(packet.SubPacket.Slot);
                return;
            }

            ItemInstance item = _itemInstanceFactory.CreateItemInstance(packet.SubPacket.VNum, packet.BagType, packet.SubPacket.Slot, packet.SubPacket.RareAmount, packet.SubPacket.UpgradeDesign);
            if (item == null)
            {
                return;
            }

            bag.AddItem(item);
        }
    }
}