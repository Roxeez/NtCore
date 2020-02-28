using Moonlight.Packet;
using Moonlight.Packet.Core.Serialization;

namespace Moonlight.Tests.Extensions
{
    public static class DeserializerExtension
    {
        internal static T Deserialize<T>(this IDeserializer deserializer, string packet) where T : class, IPacket
        {
            IPacket deserialized = deserializer.Deserialize(packet);
            return deserialized as T;
        }
    }
}