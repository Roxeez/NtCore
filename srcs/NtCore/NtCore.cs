﻿using NtCore.API;
using NtCore.API.Commands;
using NtCore.API.Logger;
using NtCore.API.Plugins;
using NtCore.API.Scheduler;

namespace NtCore
{
    public class NtCore : INtCore
    {
        public NtCore(IScheduler scheduler, ILogger logger, IPluginManager pluginManager,
            ICommandManager commandManager)
        {
            Scheduler = scheduler;
            Logger = logger;
            PluginManager = pluginManager;
            CommandManager = commandManager;
        }

        public IScheduler Scheduler { get; }
        public ILogger Logger { get; }
        public IPluginManager PluginManager { get; }
        public ICommandManager CommandManager { get; }
    }
}