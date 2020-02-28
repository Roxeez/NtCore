using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moonlight.Core.Enums;
using Moonlight.Core.Logging;
using Moonlight.Database.DAL;
using Moonlight.Database.Dto;
using Moonlight.Toolkit.Commands;
using Moonlight.Utility.Reader;
using TextReader = Moonlight.Utility.Reader.TextReader;

namespace Moonlight.Toolkit.Parsing
{
    internal class ItemParser : Parser
    {
        private readonly IRepository<ItemDto> _itemRepository;

        public ItemParser(ILogger logger, IRepository<ItemDto> itemRepository) : base(logger) => _itemRepository = itemRepository;

        public override void Parse(ParseConfiguration configuration, string directory)
        {
            Logger.Info("ITEMS");

            string itemDataPath = Path.Combine(directory, configuration.Data.Item).Replace(@"\", "/");
            if (!File.Exists(itemDataPath))
            {
                Logger.Error($"Can't found {itemDataPath} file");
                return;
            }

            Logger.Info($"Loading items from {itemDataPath}");
            TextContent content = TextReader.FromFile(itemDataPath)
                .SkipEmptyLines()
                .SkipCommentedLines("#")
                .TrimLines()
                .SplitLineContent('\t')
                .GetContent();

            IEnumerable<TextRegion> regions = content.GetRegions("VNUM");

            Logger.Info("Creating all items");
            var items = new List<ItemDto>();
            foreach (TextRegion region in regions)
            {
                TextLine firstLine = region.GetLine(x => x.StartWith("VNUM"));
                TextLine secondLine = region.GetLine(x => x.StartWith("NAME"));
                TextLine indexLine = region.GetLine(x => x.StartWith("INDEX"));
                TextLine dataLine = region.GetLine(x => x.StartWith("DATA"));

                int vnum = firstLine.GetValue<int>(1);
                string name = secondLine.GetValue(1);
                int inventoryType = indexLine.GetValue<int>(1);
                int type = indexLine.GetValue<int>(2);
                int subType = indexLine.GetValue<int>(3);
                string[] data = dataLine.GetValues().Skip(1).ToArray();

                switch (inventoryType)
                {
                    case 4:
                        inventoryType = 0;
                        break;
                    case 8:
                        inventoryType = 0;
                        break;
                    case 9:
                        inventoryType = 1;
                        break;
                    case 10:
                        inventoryType = 2;
                        break;
                }

                items.Add(new ItemDto
                {
                    Id = vnum,
                    NameKey = name,
                    BagType = (BagType)inventoryType,
                    Type = type,
                    SubType = subType,
                    Data = string.Join("|", data)
                });
            }

            _itemRepository.Clear();

            Logger.Info("Saving items to database");
            IEnumerable<ItemDto> result = _itemRepository.InsertAll(items);
            Logger.Info($"{result.Count()} items successfully parsed");
        }
    }
}