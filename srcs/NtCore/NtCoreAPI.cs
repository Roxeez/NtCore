﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using NtCore.API.Scheduler;
using NtCore.Clients;
using NtCore.Commands;
using NtCore.Events;
using NtCore.Game.Maps;
using NtCore.Game.Maps.Impl;
using NtCore.Logger;
using NtCore.Network;
using NtCore.Scheduler;

namespace NtCore
{
    public sealed class NtCoreAPI : IScheduler, IEventManager, ICommandManager, IClientManager
    {
        private static NtCoreAPI _instance;
        public static NtCoreAPI Instance => _instance ?? (_instance = new NtCoreAPI());

        private readonly ILogger _logger;
        private readonly IScheduler _scheduler;
        private readonly IEventManager _eventManager;
        private readonly IPacketManager _packetManager;
        private readonly ICommandManager _commandManager;
        private readonly IClientManager _clientManager;
        
        private NtCoreAPI()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddTransient<ILogger, ConsoleLogger>();
            services.AddTransient<IScheduler, ObservableScheduler>();

            services.AddSingleton<IClientManager, ClientManager>();
            services.AddSingleton<IEventManager, EventManager>();
            services.AddSingleton<IPacketManager, PacketManager>();
            services.AddSingleton<ICommandManager, CommandManager>();
            services.AddSingleton<IMapManager, MapManager>();
            
            foreach (Type type in typeof(IPacketHandler).Assembly.GetTypes())
            {
                if (!typeof(IPacketHandler).IsAssignableFrom(type))
                {
                    continue;
                }

                if (type.IsAbstract || type.IsInterface || !type.IsPublic)
                {
                    continue;
                }

                services.AddSingleton(typeof(IPacketHandler), type);
            }
            
            ServiceProvider core = services.BuildServiceProvider();

            _logger = core.GetService<ILogger>();
            _scheduler = core.GetService<IScheduler>();
            _eventManager = core.GetService<IEventManager>();
            _commandManager = core.GetService<ICommandManager>();
            _packetManager = core.GetService<IPacketManager>();
            _clientManager = core.GetService<IClientManager>();
            
            foreach (IPacketHandler packetHandler in core.GetServices<IPacketHandler>())
            {
                _packetManager.Register(packetHandler);
            }
            
            _logger.Information("NtCoreAPI successfully initialized.");
        }

        public ILogger GetLogger()
        {
            return _logger;
        }

        public IPacketManager GetPacketManager()
        {
            return _packetManager;
        }
        
        public void Schedule(TimeSpan delay, Action action)
        {
            _scheduler.Schedule(delay, action);
        }

        public void RegisterEventListener(IEventListener eventListener)
        {
            _eventManager.RegisterEventListener(eventListener);
        }

        public void RegisterEventListener<T>() where T : IEventListener
        {
            _eventManager.RegisterEventListener<T>();
        }

        public void CallEvent(Event e)
        {
            _eventManager.CallEvent(e);
        }

        public void RegisterCommandHandler(ICommandHandler handler)
        {
            _commandManager.RegisterCommandHandler(handler);
        }

        public void RegisterCommandHandler<T>() where T : ICommandHandler
        {
            _commandManager.RegisterCommandHandler<T>();
        }

        public void ExecuteCommand(IClient client, string command, string[] args)
        {
            _commandManager.ExecuteCommand(client, command, args);
        }

        public IEnumerable<IClient> Clients => _clientManager.Clients;
        public IClient CreateLocalClient() => _clientManager.CreateLocalClient();
        public IClient CreateRemoteClient() => _clientManager.CreateRemoteClient();
    }
}