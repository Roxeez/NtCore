using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Moonlight.Extensions;
using Moonlight.Core.Logging;
using Moonlight.Database.DAL;
using Moonlight.Database.Dto;
using Moonlight.Toolkit.Commands;
using Moonlight.Utility.Reader;
using TextReader = Moonlight.Utility.Reader.TextReader;

namespace Moonlight.Toolkit.Parsing
{
    internal class MapParser : Parser
    {
        private readonly IRepository<MapDto> _mapRepository;

        public MapParser(ILogger logger, IRepository<MapDto> mapRepository) : base(logger) => _mapRepository = mapRepository;

        public override void Parse(ParseConfiguration configuration, string directory)
        {
            Logger.Info("MAPS");

            string mapDataPath = Path.Combine(directory, configuration.Data.Map).Replace(@"\", "/");
            string mapsDirectory = Path.Combine(directory, configuration.Maps).Replace(@"\", "/");

            if (!File.Exists(mapDataPath))
            {
                Logger.Error($"Can't found {mapDataPath} file");
                return;
            }

            if (!Directory.Exists(mapsDirectory))
            {
                Logger.Error($"Can't found {mapsDirectory} directory");
                return;
            }

            Logger.Info($"Loading map names from {mapDataPath}");
            TextContent content = TextReader.FromFile(mapDataPath)
                .SkipEmptyLines()
                .SkipCommentedLines("#")
                .SkipLines(x => x.StartsWith("DATA"))
                .TrimLines()
                .SplitLineContent(' ')
                .GetContent();

            var mapNames = new Dictionary<int, string>();
            foreach (TextLine line in content.Lines)
            {
                int firstMapId = line.GetValue<int>(0);
                int secondMapId = line.GetValue<int>(1);

                string name = line.GetLastValue();

                if (firstMapId == secondMapId)
                {
                    mapNames[firstMapId] = name;
                    continue;
                }

                for (int i = firstMapId; i < secondMapId; i++)
                {
                    mapNames[i] = name;
                }
            }

            Logger.Info("Creating all maps");
            var maps = new List<MapDto>();
            foreach (string file in Directory.GetFiles(mapsDirectory))
            {
                int mapId = Convert.ToInt32(Path.GetFileName(file));

                maps.Add(new MapDto
                {
                    Id = mapId,
                    NameKey = mapNames.GetValueOrDefault(mapId) ?? string.Empty,
                    Grid = File.ReadAllBytes(file)
                });
            }

            _mapRepository.Clear();

            Logger.Info("Saving maps to database");
            IEnumerable<MapDto> result = _mapRepository.InsertAll(maps);
            Logger.Info($"{result.Count()} maps successfully parsed");
        }
    }
}