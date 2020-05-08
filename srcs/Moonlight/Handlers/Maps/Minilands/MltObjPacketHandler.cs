using System;
using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Game.Entities;
using Moonlight.Game.Factory;
using Moonlight.Game.Maps;
using Moonlight.Packet.Map.Miniland;

namespace Moonlight.Handlers.Maps.Minilands
{
    internal class MltObjPacketHandler : PacketHandler<MltObjPacket>
    {
        private readonly IMinilandObjectFactory _minilandObjectFactory;

        public MltObjPacketHandler(IMinilandObjectFactory minilandObjectFactory) => _minilandObjectFactory = minilandObjectFactory;

        protected override void Handle(Client client, MltObjPacket packet)
        {
            Character character = client.Character;

            var miniland = character?.Map as Miniland;
            if (miniland == null)
            {
                return;
            }

            miniland.Objects.Clear();

            string[] minilandObjects = packet.Content.Split(' ');
            foreach (string obj in minilandObjects)
            {
                string[] objectInfo = obj.Split('.');

                int vnum = Convert.ToInt32(objectInfo[0]);
                int slot = Convert.ToInt32(objectInfo[1]);
                var position = new Position(Convert.ToInt16(objectInfo[2]), Convert.ToInt16(objectInfo[3]));

                MinilandObject minilandObject = _minilandObjectFactory.CreateMinilandObject(vnum, slot, position);
                if (minilandObject == null)
                {
                    continue;
                }

                miniland.Objects.Add(minilandObject);
            }
        }
    }
}