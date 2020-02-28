using Microsoft.Extensions.DependencyInjection;
using Moonlight.Clients;
using Moonlight.Core;
using Moonlight.Core.Logging;
using Moonlight.Game.Entities;
using Moonlight.Game.Factory;
using Moonlight.Game.Maps;
using Moonlight.Handlers;
using Moq;

namespace Moonlight.Tests.Packet.Handling
{
    public abstract class PacketHandlingTests
    {
        protected PacketHandlingTests()
        {
            var clientMock = new Mock<Client>();

            Moonlight = new MoonlightAPI(new AppConfig
            {
                Database = "../../database.db"
            });

            SkillFactory = Moonlight.Services.GetService<ISkillFactory>();
            EntityFactory = Moonlight.Services.GetService<IEntityFactory>();
            MapFactory = Moonlight.Services.GetService<IMapFactory>();

            IPacketHandlerManager _packetHandlerManager = Moonlight.Services.GetService<IPacketHandlerManager>();

            clientMock.Setup(x => x.SendPacket(It.IsAny<string>())).Callback<string>(x => _packetHandlerManager.Handle(clientMock.Object, x));
            clientMock.Setup(x => x.ReceivePacket(It.IsAny<string>())).Callback<string>(x => _packetHandlerManager.Handle(clientMock.Object, x));

            Client = clientMock.Object;
            Client.Character = Character = new Character(new SerilogLogger(), 999, "Moonlight", Client, new Miniland("Miniland", new byte[4096]));

            Map map = MapFactory.CreateMap(1);
            map.AddEntity(Client.Character);
        }

        protected MoonlightAPI Moonlight { get; }
        protected ISkillFactory SkillFactory { get; }
        protected IEntityFactory EntityFactory { get; }
        protected IMapFactory MapFactory { get; }

        protected Client Client { get; }
        protected Character Character { get; }
    }
}