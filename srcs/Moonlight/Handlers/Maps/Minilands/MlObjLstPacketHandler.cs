using System;
using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Core.Logging;
using Moonlight.Game.Entities;
using Moonlight.Game.Factory;
using Moonlight.Game.Inventories;
using Moonlight.Game.Maps;
using Moonlight.Packet.Map.Miniland;

namespace Moonlight.Handlers.Maps.Minilands
{
    internal class MlObjLstPacketHandler : PacketHandler<MlObjLstPacket>
    {
        private readonly ILogger _logger;
        private readonly IMinilandObjectFactory _minilandObjectFactory;

        public MlObjLstPacketHandler(ILogger logger, IMinilandObjectFactory minilandObjectFactory)
        {
            _logger = logger;
            _minilandObjectFactory = minilandObjectFactory;
        }

        protected override void Handle(Client client, MlObjLstPacket packet)
        {
            Character character = client.Character;
            
            var miniland = character?.Map as Miniland;
            if (miniland == null)
            {
                _logger.Info("Not in miniland");
                return;
            }

            miniland.Objects.Clear();

            string[] minilandObjects = packet.Content.Split(' ');
            foreach (string obj in minilandObjects)
            {
                string[] objectInfo = obj.Split('.');

                int slot = Convert.ToInt32(objectInfo[0]);
                bool used = objectInfo[1] == "1";

                if (!used)
                {
                    _logger.Info($"Miniland object {slot} is not used, skipping.");
                    continue;
                }

                ItemInstance itemInstance = character.Inventory.Miniland.GetValueOrDefault(slot);
                if (itemInstance == null)
                {
                    _logger.Info($"No miniland object found in inventory at slot {slot}");
                    continue;
                }
                
                var position = new Position(Convert.ToInt16(objectInfo[2]), Convert.ToInt16(objectInfo[3]));

                MinilandObject minilandObject = _minilandObjectFactory.CreateMinilandObject(itemInstance.Item, slot, position);
                if (minilandObject == null)
                {
                    _logger.Debug("Not a miniland object");
                    continue;
                }

                miniland.Objects.Add(minilandObject);
            }
        }
    }
}