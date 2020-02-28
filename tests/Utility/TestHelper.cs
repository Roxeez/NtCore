using Microsoft.Extensions.DependencyInjection;
using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Core.Logging;
using Moonlight.Extensions;
using Moonlight.Game.Entities;
using Moonlight.Game.Maps;
using Moonlight.Packet.Core.Serialization;
using Moq;

namespace Moonlight.Tests.Utility
{
    public static class TestHelper
    {
        internal static IDeserializer CreateDeserializer()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogger();
            services.AddPacketDependencies();

            return services.BuildServiceProvider().GetService<IDeserializer>();
        }
    }
}