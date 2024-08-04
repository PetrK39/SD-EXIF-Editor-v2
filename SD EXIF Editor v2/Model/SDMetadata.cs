using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;

namespace SD_EXIF_Editor_v2.Model
{
    public class SDMetadata
    {
        public string Prompt { get; set; }
        public string NegativePrompt { get; set; }
        public int Steps { get; set; }
        public string Sampler { get; set; }
        public string ScheduleType { get; set; }
        public float CFGScale { get; set; }
        public long Seed { get; set; }
        public Size Size { get; set; }
        public SDModel Model { get; set; }
        public ObservableCollection<SDLora> Loras { get; set; }
        public string Version { get; set; }

        public SDMetadata(string rawMetadata)
        {
            var parts = rawMetadata.Split('\n');

            Prompt = parts[0];
            NegativePrompt = parts[1].Substring("Negative prompt: ".Length);

            var metadata = parts[2];

            var pattern = @"Steps: (?<steps>\d+), Sampler: (?<sampler>.+?), Schedule type: (?<schedule>.+?), CFG scale: (?<cfg>[\d.]+), Seed: (?<seed>.+?), Size: (?<size>[\dx]+), Model hash: (?<modelhash>.+?), Model: (?<modelname>.+?)(?:, Lora hashes: ""(?<loras>.+?)"")?, Version: (?<version>.+)";
            var m = Regex.Matches(metadata, pattern)[0];

            Steps = int.Parse(m.Groups["steps"].Value);
            Sampler = m.Groups["sampler"].Value;
            ScheduleType = m.Groups["schedule"].Value;
            CFGScale = float.Parse(m.Groups["cfg"].Value, CultureInfo.InvariantCulture);
            Seed = long.Parse(m.Groups["seed"].Value);
            var sizeParts = m.Groups["size"].Value.Split('x');
            Size = new Size(int.Parse(sizeParts[0]), int.Parse(sizeParts[1]));
            Model = new SDModel(m.Groups["modelname"].Value, m.Groups["modelhash"].Value);

            Loras = new ObservableCollection<SDLora>();
            var loras = m.Groups["loras"].Value;
            if (loras != "")
            {
                var loraParts = loras.Split(", ");

                foreach (var loraPart in loraParts)
                {
                    var loraPartNameHash = loraPart.Split(": ");
                    var loraStrength = Regex.Match(Prompt, $@"\<lora:{loraPartNameHash[0]}:([\d\.-]+)\>").Groups[1].Value;
                    Loras.Add(new SDLora(loraPartNameHash[0], loraPartNameHash[1], float.Parse(loraStrength, CultureInfo.InvariantCulture)));
                }
            }

            Version = m.Groups["version"].Value;
        }
    }
    public class SDModel
    {
        public string Name { get; set; }
        public string Hash { get; set; }

        public SDModel(string name, string hash)
        {
            Name = name;
            Hash = hash;
        }
        public override string ToString() => $"{Name} ({Hash})";
    }
    public class SDLora
    {
        public string Name { get; set; }
        public string Hash { get; set; }
        public float Strength { get; set; }

        public SDLora(string name, string hash, float strength)
        {
            Name = name;
            Hash = hash;
            Strength = strength;
        }
        public override string ToString() => $"{Name} [{Strength}] ({Hash})";
    }
}
