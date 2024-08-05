using SD_EXIF_Editor_v2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Service
{
    public class MetadataParserService
    {
        private readonly MessageService _messageService;
        public MetadataParserService(MessageService messageService)
        {
            _messageService = messageService;
        }
        public SDMetadata ParseFromRawMetadata(string rawMetadata)
        {
            List<int> errorCodes = new List<int>();

            var sdMetadata = new SDMetadata();

            if (rawMetadata == "") // If there's no raw metadata
            {
                _messageService.ShowInfoMessage("This file doesn't contain Automatic1111 related metadata");
                return sdMetadata;
            }

            var patternGeneralSplit = @"(?:(?<prompt>.+?)\n)?(?:Negative prompt: (?<negative>.+?)\n)?(?<metadata>.+)";

            var matchesGeneralSplit = Regex.Matches(rawMetadata, patternGeneralSplit);

            if (matchesGeneralSplit == null || matchesGeneralSplit.Count == 0)
                errorCodes.Add(1); // If we can't regex raw metadata correctly
            else
            {
                var matchGeneralSplit = matchesGeneralSplit[0];

                sdMetadata.Prompt = matchGeneralSplit.Groups["prompt"].Value;
                sdMetadata.NegativePrompt = matchGeneralSplit.Groups["negative"].Value;

                var metadata = matchGeneralSplit.Groups["metadata"].Value;

                if (string.IsNullOrWhiteSpace(metadata))
                    errorCodes.Add(2); // If metadata is empty

                var patternMetadata = @"Steps: (?<steps>\d+?), Sampler: (?<sampler>.+?), Schedule type: (?<schedule>.+?), CFG scale: (?<cfg>[\d.]+?), Seed: (?<seed>\d+?), Size: (?<size>[\dx]+?), Model hash: (?<modelhash>.+?), Model: (?<modelname>.+?)(?:, Lora hashes: ""(?<loras>.+?)"")?, Version: (?<version>.+)";
                var matchesMetadata = Regex.Matches(metadata, patternMetadata);

                if (matchesMetadata.Count == 0)
                    errorCodes.Add(3); // If we can't regex metadata correctly
                else
                {
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
                }
            }

            if (errorCodes.Any())
            {
                _messageService.ShowInfoMessage(
                    $"An error occurred while parsing data (Code {string.Join(", ", errorCodes)})\r\n" +
                    "Consider sending your raw metadata to project's github issues\r\n" +
                    "But only if you're not the one who broke it");
            }

            return sdMetadata;
        }
    }
}
