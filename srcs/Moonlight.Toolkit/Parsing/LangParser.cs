using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Moonlight.Core.Enums;
using Moonlight.Core.Logging;
using Moonlight.Database.DAL;
using Moonlight.Database.Dto;
using Moonlight.Toolkit.Commands;
using Moonlight.Translation;
using Moonlight.Utility.Reader;
using TextReader = Moonlight.Utility.Reader.TextReader;

namespace Moonlight.Toolkit.Parsing
{
    internal class LangParser : Parser
    {
        private readonly IStringRepository<TranslationDto> _translationRepository;

        public LangParser(ILogger logger, IStringRepository<TranslationDto> translationRepository) : base(logger) => _translationRepository = translationRepository;

        public override void Parse(ParseConfiguration configuration, string directory)
        {
            Logger.Info("LANGS");

            string langDirectory = Path.Combine(directory, configuration.Lang).Replace(@"\", "/");
            if (!Directory.Exists(langDirectory))
            {
                Logger.Error($"Can't found {langDirectory} directory");
                return;
            }

            var translations = new List<TranslationDto>();
            string[] directories = Directory.GetDirectories(langDirectory);
            foreach (string specificLangDirectory in directories)
            {
                string languageName = Path.GetFileName(specificLangDirectory);
                if (languageName == null)
                {
                    continue;
                }

                if (!Enum.TryParse(languageName.ToUpper(CultureInfo.InvariantCulture), out Language language))
                {
                    Logger.Warn($"Undefined language {languageName}, skipping it.");
                    continue;
                }

                Logger.Info($"Creating {language} translations");
                foreach (string file in Directory.GetFiles(specificLangDirectory))
                {
                    string rootKeyName = Path.GetFileName(file);
                    if (!Enum.TryParse(rootKeyName.ToUpper(CultureInfo.InvariantCulture), out RootKey rootKey))
                    {
                        Logger.Warn($"Undefined root key {rootKeyName}, skipping it");
                        continue;
                    }

                    TextContent content = TextReader.FromFile(file)
                        .SkipCommentedLines("#")
                        .SkipEmptyLines()
                        .TrimLines()
                        .SplitLineContent('\t')
                        .GetContent();

                    foreach (TextLine line in content.Lines)
                    {
                        string key = line.GetFirstValue();
                        string value = line.GetLastValue();

                        translations.Add(new TranslationDto
                        {
                            Id = $"{language}:{rootKey}:{key}".ToLower(CultureInfo.InvariantCulture),
                            Value = value
                        });
                    }
                }
            }

            _translationRepository.Clear();

            Logger.Info("Saving all translations to database");
            IEnumerable<TranslationDto> result = _translationRepository.InsertAll(translations);
            Logger.Info($"{result.Count()} translations successfully parsed");
        }
    }
}