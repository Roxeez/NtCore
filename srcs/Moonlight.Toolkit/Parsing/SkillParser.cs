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
    internal class SkillParser : Parser
    {
        private readonly IRepository<SkillDto> _skillRepository;

        public SkillParser(ILogger logger, IRepository<SkillDto> skillRepository) : base(logger) => _skillRepository = skillRepository;

        public override void Parse(ParseConfiguration configuration, string directory)
        {
            Logger.Info("SKILLS");

            string skillDataPath = Path.Combine(directory, configuration.Data.Skill).Replace(@"\", "/");
            if (!File.Exists(skillDataPath))
            {
                Logger.Error($"Can't found {skillDataPath} file");
                return;
            }

            Logger.Info($"Loading skills from {skillDataPath}");

            TextContent content = TextReader.FromFile(skillDataPath)
                .SkipEmptyLines()
                .SkipCommentedLines("#")
                .TrimLines()
                .SplitLineContent('\t')
                .GetContent();

            IEnumerable<TextRegion> regions = content.GetRegions("VNUM");

            Logger.Info("Creating all skills");
            var skills = new List<SkillDto>();
            foreach (TextRegion region in regions)
            {
                TextLine vnumLine = region.GetLine(x => x.StartWith("VNUM"));
                TextLine nameLine = region.GetLine(x => x.StartWith("NAME"));
                TextLine typeLine = region.GetLine(x => x.StartWith("TYPE"));
                TextLine dataLine = region.GetLine(x => x.StartWith("DATA"));
                TextLine targetLine = region.GetLine(x => x.StartWith("TARGET"));

                skills.Add(new SkillDto
                {
                    Id = vnumLine.GetValue<int>(1),
                    NameKey = nameLine.GetValue(1),
                    SkillType = (SkillType)typeLine.GetValue<int>(1),
                    CastId = typeLine.GetValue<int>(2),
                    CastTime = dataLine.GetValue<int>(5),
                    Cooldown = dataLine.GetValue<int>(6),
                    MpCost = dataLine.GetValue<int>(7),
                    TargetType = (TargetType)targetLine.GetValue<int>(1),
                    HitType = (HitType)targetLine.GetValue<int>(2),
                    Range = targetLine.GetValue<short>(3),
                    ZoneRange = targetLine.GetValue<short>(4)
                });
            }

            _skillRepository.Clear();

            Logger.Info("Saving skills to database");
            IEnumerable<SkillDto> result = _skillRepository.InsertAll(skills);
            Logger.Info($"{result.Count()} skills successfully parsed");
        }
    }
}