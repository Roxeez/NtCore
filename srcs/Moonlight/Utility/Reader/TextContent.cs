using System.Collections.Generic;

namespace Moonlight.Utility.Reader
{
    public class TextContent : TextRegion
    {
        public TextContent(IEnumerable<TextLine> lines) : base(lines)
        {
        }
    }
}