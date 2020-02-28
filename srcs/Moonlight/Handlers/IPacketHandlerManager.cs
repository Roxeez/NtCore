using System;
using System.Collections.Generic;
using Moonlight.Clients;
using Moonlight.Core.Logging;
using Moonlight.Extensions;
using Moonlight.Packet;
using Moonlight.Packet.Core.Serialization;

namespace Moonlight.Handlers
{
    internal interface IPacketHandlerManager
    {
        bool Handle(Client client, string packet);
    }

    internal class PacketHandlerManager : IPacketHandlerManager
    {
        private readonly IDeserializer _deserializer;
        private readonly IDictionary<Type, IPacketHandler> _handlers;
        private readonly ILogger _logger;

        public PacketHandlerManager(ILogger logger, IDeserializer deserializer, IEnumerable<IPacketHandler> handlers)
        {
            _logger = logger;
            _deserializer = deserializer;
            _handlers = new Dictionary<Type, IPacketHandler>();

            foreach (IPacketHandler handler in handlers)
            {
                Type type = handler.GetType().BaseType?.GenericTypeArguments[0];
                if (type == null)
                {
                    continue;
                }

                _handlers[type] = handler;
            }
        }

        public bool Handle(Client client, string packet)
        {
            try
            {
                IPacket deserialized = _deserializer.Deserialize(packet);
                if (deserialized == null || deserialized is UnknownPacket)
                {
                    return true;
                }

                if (deserialized is CommandPacket commandPacket)
                {
                    // TODO : Command manager
                    return true;
                }

                IPacketHandler handler = _handlers.GetValueOrDefault(deserialized.GetType());
                if (handler == null)
                {
                    return true;
                }

                handler.Handle(client, deserialized);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

            return true;
        }
    }
}