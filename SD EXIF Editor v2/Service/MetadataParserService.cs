using SD_EXIF_Editor_v2.Model;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SD_EXIF_Editor_v2.Service
{
    public partial class MetadataParserService
    {
        private readonly MessageService _messageService;

        private enum ErrorCodes
        {
            NoRawMetadata,
            GeneralRegexFail,
            MetadataEmpty,
            MetadataRegexFail
        }

        public MetadataParserService(MessageService messageService)
        {
            _messageService = messageService;
        }

        [GeneratedRegex("(?:(?<prompt>.+?)\n)?(?:Negative prompt: (?<negative>.+?)\n)?(?<metadata>.+)")]
        private static partial Regex GeneralSplitRegex();

        [GeneratedRegex(@"Steps: (?<steps>\d+?), Sampler: (?<sampler>.+?), Schedule type: (?<schedule>.+?), CFG scale: (?<cfg>[\d.]+?), Seed: (?<seed>\d+?), Size: (?<size>[\dx]+?), Model hash: (?<modelhash>.+?), Model: (?<modelname>.+?)(?:, Lora hashes: ""(?<loras>.+?)"")?, Version: (?<version>.+)")]
        private static partial Regex MetadataRegex();

        public SDMetadata ParseFromRawMetadata(string rawMetadata)
        {
            var sdMetadata = new SDMetadata();
            List<ErrorCodes> errorCodes = [];

            if (rawMetadata == "")
            {
                errorCodes.Add(ErrorCodes.MetadataEmpty);

                DisplayErrorMessage(errorCodes);
                return sdMetadata;
            }

            var matchesGeneralSplit = GeneralSplitRegex().Matches(rawMetadata);

            if (matchesGeneralSplit.Count == 0)
            {
                errorCodes.Add(ErrorCodes.GeneralRegexFail); // If we can't regex raw metadata correctly

                DisplayErrorMessage(errorCodes);
                return sdMetadata;
            }

            var matchGeneralSplit = matchesGeneralSplit[0];

            sdMetadata.Prompt = matchGeneralSplit.Groups["prompt"].Value;
            sdMetadata.NegativePrompt = matchGeneralSplit.Groups["negative"].Value;

            var metadata = matchGeneralSplit.Groups["metadata"].Value;

            if (string.IsNullOrWhiteSpace(metadata))
            {
                errorCodes.Add(ErrorCodes.MetadataEmpty); // If metadata is empty
                DisplayErrorMessage(errorCodes);
                return sdMetadata;
            }

            var matchesMetadata = MetadataRegex().Matches(metadata);

            if (matchesMetadata.Count == 0)
            {
                errorCodes.Add(ErrorCodes.MetadataRegexFail); // If we can't regex metadata correctly
                DisplayErrorMessage(errorCodes);
                return sdMetadata;
            }

            var matchMetadata = matchesMetadata[0];

            sdMetadata.Steps = int.Parse(matchMetadata.Groups["steps"].Value);
            sdMetadata.Sampler = matchMetadata.Groups["sampler"].Value;
            sdMetadata.ScheduleType = matchMetadata.Groups["schedule"].Value;
            sdMetadata.CFGScale = float.Parse(matchMetadata.Groups["cfg"].Value, CultureInfo.InvariantCulture);
            sdMetadata.Seed = long.Parse(matchMetadata.Groups["seed"].Value);

            var sizeParts = matchMetadata.Groups["size"].Value.Split('x');
            sdMetadata.Size = new Size(int.Parse(sizeParts[0]), int.Parse(sizeParts[1]));

            sdMetadata.Model = new SDModel(matchMetadata.Groups["modelname"].Value, matchMetadata.Groups["modelhash"].Value);

            var loras = matchMetadata.Groups["loras"].Value;

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

            sdMetadata.Version = matchMetadata.Groups["version"].Value;

            DisplayErrorMessage(errorCodes);

            return sdMetadata;
        }

        private void DisplayErrorMessage(IEnumerable<ErrorCodes> errorCodes)
        {
            if (errorCodes.Any())
            {
                _messageService.ShowInfoMessage(
                    $"An error occurred while parsing data (Code: {string.Join(", ", errorCodes)})\r\n" +
                    "Consider sending your raw metadata to project's github issues\r\n" +
                    "But only if you're not the one who broke it");
            }
        }
    }
}
