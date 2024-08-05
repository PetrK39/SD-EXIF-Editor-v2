using System.Collections.ObjectModel;
using System.Drawing;

namespace SD_EXIF_Editor_v2.Model
{
    public class SDMetadata
    {
        public string? Prompt { get; set; }
        public string? NegativePrompt { get; set; }
        public int? Steps { get; set; }
        public string? Sampler { get; set; }
        public string? ScheduleType { get; set; }
        public float? CFGScale { get; set; }
        public long? Seed { get; set; }
        public Size? Size { get; set; }
        public SDModel? Model { get; set; }
        public ObservableCollection<SDLora> Loras { get; set; }
        public string? Version { get; set; }
        public SDMetadata()
        {
            Loras = [];
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
