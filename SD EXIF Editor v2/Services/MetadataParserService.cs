using Microsoft.Extensions.Logging;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SD_EXIF_Editor_v2.Service
{
    public partial class MetadataParserService : IMetadataParserService
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<MetadataParserService> _logger;

        private enum ErrorCodes
        {
            NoRawMetadata,
            GeneralRegexFail,
            MetadataEmpty,
            MetadataRegexFail,
            MetadataLorasRegexFail,
            LoraStrengthParseFail,
            LoraStrengthRegexFail,
        }

        public MetadataParserService(IMessageService messageService, ILogger<MetadataParserService> logger)
        {
            _messageService = messageService;
            _logger = logger;
            _logger.LogTrace("MetadataParserService initialized.");
        }

        [GeneratedRegex(@"\s*(?<key>\w[\w \-/]+):\s*(?<value>""(?:\\.|[^\\""])+""|[^,]*)(?:,|$)")]
        private static partial Regex MetadataRegex();

        public SDMetadata ParseFromRawMetadata(string rawMetadata)
        {
            _logger.LogTrace("Entering ParseFromRawMetadata method.");
            _logger.LogDebug($"Trying to parse raw metadata: {rawMetadata}");

            List<ErrorCodes> errorCodes = [];

            var (prompt, negative, metadata) = ParseGeneral(rawMetadata, errorCodes);

            if (string.IsNullOrWhiteSpace(metadata))
            {
                errorCodes.Add(ErrorCodes.MetadataEmpty);
                _logger.LogWarning("Metadata is empty.");

                DisplayErrorMessage(errorCodes);
                _logger.LogTrace("Exiting ParseFromRawMetadata method as metadata is empty.");
                return new SDMetadata { Prompt = prompt, NegativePrompt = negative };
            }

            var matchesMetadata = MetadataRegex().Matches(metadata);

            if (matchesMetadata.Count == 0)
            {
                errorCodes.Add(ErrorCodes.MetadataRegexFail);
                _logger.LogError("Failed to match metadata regex.");

                DisplayErrorMessage(errorCodes);
                _logger.LogTrace("Exiting ParseFromRawMetadata method as we can't regex metadata.");
                return new SDMetadata { Prompt = prompt, NegativePrompt = negative };
            }

            var sdModel = new SDModel(matchesMetadata.Single(i => i.Groups["key"].Value == "Model").Groups["value"].Value,
                matchesMetadata.Single(i => i.Groups["key"].Value == "Model hash").Groups["value"].Value);

            var sdMetadata = new SDMetadata { Prompt = prompt, NegativePrompt = negative, Model = sdModel };

            foreach (Match match in matchesMetadata)
            {
                var key = match.Groups["key"].Value;

                if (key == "Model" || key == "Model hash")
                    continue; // skip as we have dedicated property for the model

                var value = match.Groups["value"].Value;

                if (key == "Lora hashes")
                {
                    value = value[1..^1];
                    ProcessLoraHashes(value, sdMetadata, errorCodes);
                }
                else
                {
                    sdMetadata.MetadataProperties.Add(key, value);
                }
            }

            DisplayErrorMessage(errorCodes);

            _logger.LogTrace("Exiting ParseFromRawMetadata method.");
            return sdMetadata;
        }

        private void ProcessLoraHashes(string value, SDMetadata sdMetadata, List<ErrorCodes> errorCodes)
        {
            _logger.LogTrace("Entering ProcessLoraHashes method.");

            if (string.IsNullOrEmpty(value))
            {
                _logger.LogError("Failed to regex loras in metadata");
                errorCodes.Add(ErrorCodes.MetadataLorasRegexFail);
                _logger.LogTrace("Exiting ProcessLoraHashes method as value is empty.");
                return;
            }

            var matchesLoras = MetadataRegex().Matches(value);
            foreach (Match matchLora in matchesLoras)
            {
                ProcessLoraMatch(matchLora, sdMetadata, errorCodes);
            }

            _logger.LogTrace("Exiting ProcessLoraHashes method.");
        }

        void ProcessLoraMatch(Match matchLora, SDMetadata sdMetadata, List<ErrorCodes> errorCodes)
        {
            _logger.LogTrace("Entering ProcessLoraMatch method.");

            var loraKey = matchLora.Groups["key"].Value;
            var loraValue = matchLora.Groups["value"].Value;
            var loraStrengthMatch = Regex.Match(sdMetadata.Prompt, $@"\<lora:{loraKey}:([\d\.-]+)\>");

            if (!loraStrengthMatch.Success)
            {
                _logger.LogError($"Failed to regex lora's ({loraKey}) strength");
                errorCodes.Add(ErrorCodes.LoraStrengthRegexFail);
                sdMetadata.Loras.Add(new SDLora(loraKey, loraValue, float.NaN));
                _logger.LogTrace("Exiting ProcessLoraMatch method as regex failed.");
                return;
            }

            if (!float.TryParse(loraStrengthMatch.Groups[1].Value, CultureInfo.InvariantCulture, out float loraStrength))
            {
                _logger.LogError($"Failed to parse lora's ({loraKey}) strength");
                errorCodes.Add(ErrorCodes.LoraStrengthParseFail);
                sdMetadata.Loras.Add(new SDLora(loraKey, loraValue, float.NaN));
                _logger.LogTrace("Exiting ProcessLoraMatch method as parsing failed.");
                return;
            }

            sdMetadata.Loras.Add(new SDLora(loraKey, loraValue, loraStrength));

            _logger.LogTrace("Exiting ProcessLoraMatch method.");
        }

        private (string Prompt, string NegativePrompt, string Metadata) ParseGeneral(string rawMetadata, List<ErrorCodes> errorCodes)
        {
            _logger.LogTrace("Entering ParseGeneral method.");

            if (string.IsNullOrEmpty(rawMetadata))
            {
                _logger.LogWarning("Raw metadata is empty.");
                errorCodes.Add(ErrorCodes.NoRawMetadata);
                _logger.LogTrace("Exiting ParseGeneral method as raw metadata is empty.");
                return ("", "", "");
            }

            string prompt = "", negative = "";

            var lines = rawMetadata.Split('\n');

            bool doneWithPrompt = false;
            foreach (var line in lines.SkipLast(1))
            {
                var l = line.Trim();
                if (l.StartsWith("Negative prompt: "))
                {
                    doneWithPrompt = true;
                    l = l.Substring("Negative prompt: ".Length).Trim();
                }
                if (doneWithPrompt)
                {
                    negative += (negative == "" ? "" : "\n") + l;
                }
                else
                {
                    prompt += (prompt == "" ? "" : "\n") + l;
                }

            }

            _logger.LogTrace("Exiting ParseGeneral method.");
            return (prompt, negative, lines.Last());
        }

        private void DisplayErrorMessage(IEnumerable<ErrorCodes> errorCodes)
        {
            _logger.LogTrace("Entering DisplayErrorMessage method.");

            if (errorCodes.Any())
            {
                var errorCodesJoined = string.Join(", ", errorCodes);
                _logger.LogWarning($"Encountered errors during parsing: {errorCodesJoined}");
                _messageService.ShowInfoMessage(
                    $"An error occurred while parsing data (Code: {errorCodesJoined})\r\n" +
                    "Consider sending your raw metadata to project's github issues\r\n" +
                    "But only if you're not the one who broke it");
            }

            _logger.LogTrace("Exiting DisplayErrorMessage method.");
        }
    }
}