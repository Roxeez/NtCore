using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Moonlight.Extensions;
using Moonlight.Packet.Core.Attributes;
using Moonlight.Utility;

namespace Moonlight.Packet.Core
{
    internal class ReflectionCache : IReflectionCache
    {
        private readonly Dictionary<string, CachedType> _cacheByHeader;
        private readonly Dictionary<Type, CachedType> _cacheByType;

        public ReflectionCache()
        {
            _cacheByHeader = new Dictionary<string, CachedType>();
            _cacheByType = new Dictionary<Type, CachedType>();

            foreach (Type type in typeof(IPacket).Assembly.GetTypes())
            {
                if (!typeof(IPacket).IsAssignableFrom(type))
                {
                    continue;
                }

                if (type.IsAbstract || type.IsInterface)
                {
                    continue;
                }

                Cache(type);
            }
        }

        public CachedType GetCachedType(string packetHeader) => _cacheByHeader.GetValueOrDefault(packetHeader);

        public CachedType GetCachedType(Type type) => _cacheByType.GetValueOrDefault(type);

        private void Cache(Type type)
        {
            if (!typeof(IPacket).IsAssignableFrom(type))
            {
                return;
            }

            PacketHeaderAttribute packetHeaderAttribute = type.GetCustomAttribute<PacketHeaderAttribute>();

            var typeCreator = new CachedType
            {
                PacketHeaderAttribute = packetHeaderAttribute,
                PacketType = type,
                Constructor = Expression.Lambda(Expression.New(type)).Compile(),
                Properties = GetProperties(type)
            };

            if (packetHeaderAttribute != null)
            {
                _cacheByHeader[packetHeaderAttribute.Header] = typeCreator;
            }

            _cacheByType[type] = typeCreator;

            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                Cache(propertyInfo.PropertyType);
            }
        }

        private static List<PropertyData> GetProperties(Type type)
        {
            var properties = new List<PropertyData>();
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                PacketIndexAttribute packetIndexAttribute = propertyInfo.GetCustomAttribute<PacketIndexAttribute>();
                if (packetIndexAttribute == null)
                {
                    continue;
                }

                properties.Add(new PropertyData
                {
                    PropertyType = propertyInfo.PropertyType,
                    PacketIndexAttribute = packetIndexAttribute,
                    Setter = FastReflection.GetPropertySetter(type, propertyInfo),
                    Getter = FastReflection.GetPropertyGetter(type, propertyInfo)
                });
            }

            return properties;
        }
    }
}