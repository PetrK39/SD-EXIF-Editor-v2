﻿using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SD_EXIF_Editor_v2.Service
{
    public partial class MetadataParserService : IMetadataParserService
    {
        private readonly MessageService _messageService;
        private readonly ILoggingService _loggingService;

        private enum ErrorCodes
        {
            NoRawMetadata,
            GeneralRegexFail,
            MetadataEmpty,
            MetadataRegexFail
        }

        public MetadataParserService(MessageService messageService, ILoggingService loggingService)
        {
            _messageService = messageService;
            _loggingService = loggingService;
            _loggingService.Trace("MetadataParserService initialized.");
        }

        [GeneratedRegex("(?:(?<prompt>.+?)\n)?(?:Negative prompt: (?<negative>.+?)\n)?(?<metadata>.+)")]
        private static partial Regex GeneralSplitRegex();

        [GeneratedRegex(@"Steps: (?<steps>\d+?), Sampler: (?<sampler>.+?), Schedule type: (?<schedule>.+?), CFG scale: (?<cfg>[\d.]+?), Seed: (?<seed>\d+?), Size: (?<size>[\dx]+?), Model hash: (?<modelhash>.+?), Model: (?<modelname>.+?)(?:, Lora hashes: ""(?<loras>.+?)"")?, Version: (?<version>.+)")]
        private static partial Regex MetadataRegex();

        public SDMetadata ParseFromRawMetadata(string rawMetadata)
        {
            _loggingService.Trace("Entering ParseFromRawMetadata method.");
            _loggingService.Debug($"Trying to parse raw metadata: {rawMetadata}");

            List<ErrorCodes> errorCodes = [];

            if (rawMetadata == "")
            {
                errorCodes.Add(ErrorCodes.MetadataEmpty);
                _loggingService.Warn("Raw metadata is empty.");

                DisplayErrorMessage(errorCodes);
                return new SDMetadata();
            }

            var matchesGeneralSplit = GeneralSplitRegex().Matches(rawMetadata);

            if (matchesGeneralSplit.Count == 0)
            {
                errorCodes.Add(ErrorCodes.GeneralRegexFail);
                _loggingService.Error("Failed to match general split regex.");

                DisplayErrorMessage(errorCodes);
                return new SDMetadata();
            }

            var matchGeneralSplit = matchesGeneralSplit[0];

            var sdPrompt = matchGeneralSplit.Groups["prompt"].Value;
            var sdNegativePrompt = matchGeneralSplit.Groups["negative"].Value;

            var metadata = matchGeneralSplit.Groups["metadata"].Value;

            if (string.IsNullOrWhiteSpace(metadata))
            {
                errorCodes.Add(ErrorCodes.MetadataEmpty);
                _loggingService.Warn("Metadata is empty.");

                DisplayErrorMessage(errorCodes);
                return new SDMetadata { Prompt = sdPrompt, NegativePrompt = sdNegativePrompt };
            }

            var matchesMetadata = MetadataRegex().Matches(metadata);

            if (matchesMetadata.Count == 0)
            {
                errorCodes.Add(ErrorCodes.MetadataRegexFail);
                _loggingService.Error("Failed to match metadata regex.");

                DisplayErrorMessage(errorCodes);
                return new SDMetadata { Prompt = sdPrompt, NegativePrompt = sdNegativePrompt };
            }

            var matchMetadata = matchesMetadata[0];

            var sdSteps = int.Parse(matchMetadata.Groups["steps"].Value);
            var sdSampler = matchMetadata.Groups["sampler"].Value;
            var sdScheduleType = matchMetadata.Groups["schedule"].Value;
            var sdCFGScale = float.Parse(matchMetadata.Groups["cfg"].Value, CultureInfo.InvariantCulture);
            var sdSeed = long.Parse(matchMetadata.Groups["seed"].Value);

            var sizeParts = matchMetadata.Groups["size"].Value.Split('x');
            var sdSize = new Size(int.Parse(sizeParts[0]), int.Parse(sizeParts[1]));

            var sdModel = new SDModel(matchMetadata.Groups["modelname"].Value, matchMetadata.Groups["modelhash"].Value);

            var loras = matchMetadata.Groups["loras"].Value;

            var sdVersion = matchMetadata.Groups["version"].Value;

            var sdMetadata = new SDMetadata
            {
                Prompt = sdPrompt,
                NegativePrompt = sdNegativePrompt,
                Steps = sdSteps,
                Sampler = sdSampler,
                ScheduleType = sdScheduleType,
                CFGScale = sdCFGScale,
                Seed = sdSeed,
                Size = sdSize,
                Model = sdModel,
                Version = sdVersion,
            };

            if (loras != "")
            {
                var loraParts = loras.Split(", ");

                foreach (var loraPart in loraParts)
                {
                    var loraPartNameHash = loraPart.Split(": ");
                    var loraStrength = Regex.Match(sdMetadata.Prompt, $@"\<lora:{loraPartNameHash[0]}:([\d\.-]+)\>").Groups[1].Value;
                    sdMetadata.Loras.Add(new SDLora(loraPartNameHash[0], loraPartNameHash[1], float.Parse(loraStrength, CultureInfo.InvariantCulture)));
                }
            }

            DisplayErrorMessage(errorCodes);

            _loggingService.Trace("Exiting ParseFromRawMetadata method.");
            return sdMetadata;
        }

        private void DisplayErrorMessage(IEnumerable<ErrorCodes> errorCodes)
        {
            if (errorCodes.Any())
            {
                var errorCodesJoined = string.Join(", ", errorCodes);
                _loggingService.Warn($"Encountered errors during parsing: {errorCodesJoined}");
                _messageService.ShowInfoMessage(
                    $"An error occurred while parsing data (Code: {errorCodesJoined})\r\n" +
                    "Consider sending your raw metadata to project's github issues\r\n" +
                    "But only if you're not the one who broke it");
            }
        }
    }
}
