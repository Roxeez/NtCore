using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Moonlight.Clients.Local;
using Moonlight.Core.Interop;
using Moonlight.Handlers;

namespace Moonlight.Clients
{
    internal interface IClientManager
    {
        Client CreateLocalClient();
    }

    internal class ClientManager : IClientManager
    {
        private readonly IPacketHandlerManager _packetHandlerManager;

        public ClientManager(IPacketHandlerManager packetHandlerManager) => _packetHandlerManager = packetHandlerManager;

        public Client CreateLocalClient()
        {
            IEnumerable<IntPtr> windows = User32.FindWindowsWithTitle("NosTale");
            IntPtr currentWindow = windows.FirstOrDefault(x =>
            {
                User32.GetWindowThreadProcessId(x, out uint pid);
                return Process.GetCurrentProcess().Id == pid;
            });

            if (currentWindow == IntPtr.Zero)
            {
                throw new InvalidOperationException("Can't find window");
            }

            Client client = new LocalClient(new Window(currentWindow));

            client.PacketReceived += x => _packetHandlerManager.Handle(client, x);
            client.PacketSend += x => _packetHandlerManager.Handle(client, x);

            return client;
        }
    }
}