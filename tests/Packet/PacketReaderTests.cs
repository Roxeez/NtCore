using Moonlight.Packet.Core;
using Moonlight.Tests.Extensions;
using NFluent;
using Xunit;

namespace Moonlight.Tests.Packet
{
    public class PacketReaderTests
    {
        private readonly IPacketReader _packetReader;

        public PacketReaderTests() => _packetReader = new PacketReader();

        [Fact]
        public void Read_Packet_Without_Content()
        {
            PacketOutput output = _packetReader.Read("mapclean");

            Check.That(output.Header).Is("mapclean");
            Check.That(output.Content).IsNullOrEmpty();
        }

        [Fact]
        public void Read_Packet_With_Content()
        {
            PacketOutput output = _packetReader.Read("lev 1 5000 2 10000");
            
            Check.That(output.Header).Is("lev");
            Check.That(output.Content).IsNotNullOrEmpty();

            string[] contentSplit = output.Content.Split(' ');
            
            Check.That(contentSplit).CountIs(4);
            Check.That(contentSplit).HasElementAt(0).Which.Is("1");
            Check.That(contentSplit).HasElementAt(1).Which.Is("5000");
            Check.That(contentSplit).HasElementAt(2).Which.Is("2");
            Check.That(contentSplit).HasElementAt(3).Which.Is("10000");
        }

        [Fact]
        public void Read_Packet_With_Complex_Content()
        {
            PacketOutput output = _packetReader.Read("inv 1 0.5.4.1 1.2.9.7");
            
            Check.That(output.Header).Is("inv");
            Check.That(output.Content).IsNotNullOrEmpty();

            string[] contentSplit = output.Content.Split(' ');

            Check.That(contentSplit).CountIs(3);
            Check.That(contentSplit).HasElementAt(0).WhichIs("1");
            Check.That(contentSplit).HasElementAt(1).WhichIs("0.5.4.1");
            Check.That(contentSplit).HasElementAt(2).WhichIs("1.2.9.7");
        }
    }
}