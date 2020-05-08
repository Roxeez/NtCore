using Moonlight.Clients;
using Moonlight.Core.Logging;
using Moonlight.Game.Factory;
using Moonlight.Game.Inventories;
using Moonlight.Packet.Character.Inventory;

namespace Moonlight.Handlers.Characters.Inventories
{
    internal class InvPacketHandler : PacketHandler<InvPacket>
    {
        private readonly IItemInstanceFactory _itemInstanceFactory;
        private readonly ILogger _logger;

        public InvPacketHandler(ILogger logger, IItemInstanceFactory itemInstanceFactory)
        {
            _logger = logger;
            _itemInstanceFactory = itemInstanceFactory;
        }

        protected override void Handle(Client client, InvPacket packet)
        {
            if (client.Character == null || client.Character.Inventory == null)
            {
                return;
            }


            Bag bag = client.Character.Inventory.GetBag(packet.BagType);

            if (bag == null)
            {
                _logger.Error($"Can't found bad {packet.BagType}");
                return;
            }

            foreach (IvnSubPacket sub in packet.SubPackets)
            {
                ItemInstance existingItem = bag.GetValueOrDefault(sub.Slot);
                if (existingItem == null)
                {
                    ItemInstance item = _itemInstanceFactory.CreateItemInstance(sub.VNum, sub.RareAmount);
                    if (item == null)
                    {
                        return;
                    }

                    bag[sub.Slot] = item;
                    continue;
                }

                existingItem.Amount = sub.RareAmount;
            }

            _logger.Info($"{packet.BagType} bag initialized.");
        }
    }
}