using Moonlight.Clients;
using Moonlight.Game.Entities;
using Moonlight.Game.Factory;
using Moonlight.Game.Inventories;
using Moonlight.Packet.Character.Inventory;

namespace Moonlight.Handlers.Characters.Inventories
{
    internal class IvnPacketHandler : PacketHandler<IvnPacket>
    {
        private readonly IItemInstanceFactory _itemInstanceFactory;

        public IvnPacketHandler(IItemInstanceFactory itemInstanceFactory) => _itemInstanceFactory = itemInstanceFactory;

        protected override void Handle(Client client, IvnPacket packet)
        {
            if (client.Character == null || client.Character.Inventory == null)
            {
                return;
            }

            Character character = client.Character;
            IvnSubPacket ivn = packet.SubPacket;

            Bag bag = character.Inventory.GetBag(packet.BagType);
            if (bag == null)
            {
                return;
            }

            if (ivn.VNum == -1)
            {
                bag.Remove(ivn.Slot);
                return;
            }

            ItemInstance existingItem = bag.GetValueOrDefault(ivn.Slot);
            if (existingItem == null || existingItem.Item.Vnum == ivn.VNum)
            {
                ItemInstance item = _itemInstanceFactory.CreateItemInstance(packet.SubPacket.VNum, packet.SubPacket.RareAmount);
                if (item == null)
                {
                    return;
                }

                bag[ivn.Slot] = item;
                return;
            }

            existingItem.Amount = packet.SubPacket.RareAmount;
        }
    }
}