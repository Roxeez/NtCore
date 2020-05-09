using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moonlight.Clients;
using Moonlight.Game.Raids;

namespace Moonlight.Event.Raids
{
    public class RaidInitializedEvent : IEventNotification
    {
        public RaidInitializedEvent(Client emitter) => Emitter = emitter;

        public Raid Raid { get; set; }

        public Client Emitter { get; }
    }
}
