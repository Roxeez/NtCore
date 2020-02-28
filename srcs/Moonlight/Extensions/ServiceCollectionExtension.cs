using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Moonlight.Core;
using Moonlight.Core.Logging;
using Moonlight.Database;
using Moonlight.Database.DAL;
using Moonlight.Database.Dto;
using Moonlight.Database.Entities;
using Moonlight.Game.Factory;
using Moonlight.Game.Factory.Impl;
using Moonlight.Packet.Core;
using Moonlight.Packet.Core.Serialization;
using Moonlight.Utility.Conversion;
using Moonlight.Utility.Conversion.Converters;

namespace Moonlight.Extensions
{
    public static class ServiceCollectionExtension
    {
        internal static void AddFactories(this IServiceCollection services)
        {
            services.AddTransient<IMapFactory, MapFactory>();
            services.AddTransient<IEntityFactory, EntityFactory>();
            services.AddTransient<IItemFactory, ItemFactory>();
            services.AddTransient<IMinilandObjectFactory, MinilandObjectFactory>();
            services.AddTransient<ISkillFactory, SkillFactory>();
            services.AddTransient<IItemInstanceFactory, ItemInstanceFactory>();
        }

        internal static void AddPacketDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IConversionFactory, ConversionFactory>();
            services.AddSingleton<IReflectionCache, ReflectionCache>();
            services.AddImplementingTypes<IConverter>();
            services.AddTransient<IDeserializer, Deserializer>();
        }

        internal static void AddDatabaseDependencies(this IServiceCollection services, AppConfig config)
        {
            services.AddTransient<AppConfig>(x => config);
            services.AddTransient<IContextFactory<MoonlightContext>, SqliteContextFactory>();
            services.AddTransient(typeof(IMapper<,>), typeof(MapsterMapper<,>));

            services.AddSingleton<IStringRepository<TranslationDto>, CachedStringRepository<Database.Entities.Translation, TranslationDto, MoonlightContext>>();
            services.AddTransient<IRepository<MapDto>, Repository<Map, MapDto, MoonlightContext>>();
            services.AddTransient<IRepository<ItemDto>, Repository<Item, ItemDto, MoonlightContext>>();
            services.AddTransient<IRepository<MonsterDto>, CachedRepository<Monster, MonsterDto, MoonlightContext>>();
            services.AddTransient<IRepository<SkillDto>, Repository<Skill, SkillDto, MoonlightContext>>();
        }

        public static void AddImplementingTypes<T>(this IServiceCollection services)
        {
            IEnumerable<Type> types = typeof(T).Assembly.GetAssignableTypes<T>();
            foreach (Type type in types)
            {
                services.AddTransient(typeof(T), type);
            }
        }

        public static void AddImplementingTypes<T>(this IServiceCollection services, Assembly assembly)
        {
            IEnumerable<Type> types = assembly.GetAssignableTypes<T>();
            foreach (Type type in types)
            {
                services.AddTransient(typeof(T), type);
            }
        }

        internal static void AddLogger(this IServiceCollection services)
        {
            services.AddTransient<ILogger, SerilogLogger>();
        }
    }
}