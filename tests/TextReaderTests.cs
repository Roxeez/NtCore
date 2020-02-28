using Moonlight.Tests.Extensions;
using Moonlight.Utility.Reader;
using NFluent;
using Xunit;

namespace Moonlight.Tests
{
    public class TextReaderTests
    {
        private const string Text = @"My first line    
# My second line
        My third line
    My fourth line
Skip me";

        [Fact]
        public void All_Possibilities()
        {
            TextContent content = TextReader.FromString(Text)
                .SkipCommentedLines("#")
                .SkipLines(x => x.Equals("Skip me"))
                .SkipEmptyLines()
                .TrimLines()
                .SplitLineContent(' ')
                .GetContent();

            Check.That(content.Lines).CountIs(3);

            Check.That(content.Lines).HasElementAt(0).WhichMatch(x => x.AsString().Equals("My first line"));
            Check.That(content.Lines).HasElementAt(1).WhichMatch(x => x.AsString().Equals("My third line"));
            Check.That(content.Lines).HasElementAt(2).WhichMatch(x => x.AsString().Equals("My fourth line"));

            foreach (TextLine line in content.Lines)
            {
                Check.That(line.GetValues()).CountIs(3);
            }
        }

        [Fact]
        public void Just_Get_Content()
        {
            TextContent content = TextReader.FromString(Text)
                .GetContent();

            Check.That(content.Lines).CountIs(5);
            Check.That(content.Lines).HasElementAt(0).WhichMatch(x => x.AsString().Equals("My first line    "));
            Check.That(content.Lines).HasElementAt(1).WhichMatch(x => x.AsString().Equals("# My second line"));
            Check.That(content.Lines).HasElementAt(2).WhichMatch(x => x.AsString().Equals("        My third line"));
            Check.That(content.Lines).HasElementAt(3).WhichMatch(x => x.AsString().Equals("    My fourth line"));
            Check.That(content.Lines).HasElementAt(4).WhichMatch(x => x.AsString().Equals("Skip me"));
        }

        [Fact]
        public void Skip_Commented_Lines()
        {
            TextContent content = TextReader.FromString(Text)
                .SkipCommentedLines("#")
                .GetContent();

            Check.That(content.Lines).CountIs(4);
            Check.That(content.Lines).HasElementAt(0).WhichMatch(x => x.AsString().Equals("My first line    "));
            Check.That(content.Lines).HasElementAt(1).WhichMatch(x => x.AsString().Equals("        My third line"));
            Check.That(content.Lines).HasElementAt(2).WhichMatch(x => x.AsString().Equals("    My fourth line"));
            Check.That(content.Lines).HasElementAt(3).WhichMatch(x => x.AsString().Equals("Skip me"));
        }

        [Fact]
        public void Skip_SkipMe_Line()
        {
            TextContent content = TextReader.FromString(Text)
                .SkipLines(x => x.Equals("Skip me"))
                .GetContent();

            Check.That(content.Lines).CountIs(4);
            Check.That(content.Lines).HasElementAt(0).WhichMatch(x => x.AsString().Equals("My first line    "));
            Check.That(content.Lines).HasElementAt(1).WhichMatch(x => x.AsString().Equals("# My second line"));
            Check.That(content.Lines).HasElementAt(2).WhichMatch(x => x.AsString().Equals("        My third line"));
            Check.That(content.Lines).HasElementAt(3).WhichMatch(x => x.AsString().Equals("    My fourth line"));
        }

        [Fact]
        public void Trim_Lines()
        {
            TextContent content = TextReader.FromString(Text)
                .TrimLines()
                .GetContent();

            Check.That(content.Lines).CountIs(5);
            Check.That(content.Lines).HasElementAt(0).WhichMatch(x => x.AsString().Equals("My first line"));
            Check.That(content.Lines).HasElementAt(1).WhichMatch(x => x.AsString().Equals("# My second line"));
            Check.That(content.Lines).HasElementAt(2).WhichMatch(x => x.AsString().Equals("My third line"));
            Check.That(content.Lines).HasElementAt(3).WhichMatch(x => x.AsString().Equals("My fourth line"));
            Check.That(content.Lines).HasElementAt(4).WhichMatch(x => x.AsString().Equals("Skip me"));
        }
    }
}