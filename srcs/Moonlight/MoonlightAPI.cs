using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Core.Interop;
using Moonlight.Extensions;
using Moonlight.Core.Logging;
using Moonlight.Event;
using Moonlight.Handlers;
using Moonlight.Translation;

[assembly: InternalsVisibleTo("Moonlight.Tests")]
[assembly: InternalsVisibleTo("Moonlight.Toolkit")]
namespace Moonlight
{
    public sealed class MoonlightAPI
    {
        internal static SynchronizationContext Context { get; private set; }
        
        private readonly IClientManager _clientManager;
        private readonly ILanguageService _languageService;
        private readonly IPacketHandlerManager _packetHandlerManager;
        private readonly IEventManager _eventManager;

        internal IServiceProvider Services { get; }
        
        public MoonlightAPI() : this(new AppConfig())
        {
        }

        public MoonlightAPI(SynchronizationContext context) : this()
        {
            Context = context;
        }

        internal MoonlightAPI(AppConfig config)
        {
            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddLogger();
            serviceCollection.AddPacketDependencies();
            serviceCollection.AddDatabaseDependencies(config);
            serviceCollection.AddFactories();

            serviceCollection.AddSingleton<ILanguageService, LanguageService>();
            serviceCollection.AddSingleton<IClientManager, ClientManager>();
            serviceCollection.AddSingleton<IPacketHandlerManager, PacketHandlerManager>();
            serviceCollection.AddSingleton<IEventManager, EventManager>();

            serviceCollection.AddImplementingTypes<IPacketHandler>();

            Services = serviceCollection.BuildServiceProvider();

            _clientManager = Services.GetService<IClientManager>();
            _packetHandlerManager = Services.GetService<IPacketHandlerManager>();
            _languageService = Services.GetService<ILanguageService>();
            _eventManager = Services.GetService<IEventManager>();

            Logger = Services.GetService<ILogger>();
        }

        public Language Language
        {
            get => _languageService.Language;
            set => _languageService.Language = value;
        }

        public ILogger Logger { get; }

        public Client CreateLocalClient() => _clientManager.CreateLocalClient();

        public void AddListener<T>(EventListener<T> listener) where T : IEventNotification
        {
            _eventManager.RegisterListener(listener);
        }

        public void AllocConsole()
        {
            Kernel32.AllocConsole();
        }
    }
}