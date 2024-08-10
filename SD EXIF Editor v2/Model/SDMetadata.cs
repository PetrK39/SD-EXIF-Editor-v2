using System.Drawing;

namespace SD_EXIF_Editor_v2.Model
{
    public class SDMetadata
    {
        public string? Prompt { get; init; }
        public string? NegativePrompt { get; init; }
        public int? Steps { get; init; }
        public string? Sampler { get; init; }
        public string? ScheduleType { get; init; }
        public float? CFGScale { get; init; }
        public long? Seed { get; init; }
        public Size? Size { get; init; }
        public SDModel? Model { get; init; }
        public List<SDLora> Loras { get; init; }
        public string? Version { get; init; }
        public SDMetadata()
        {
            Loras = [];
        }
    }
    public class SDModel
    {
        public string Name { get; init; }
        public string Hash { get; init; }

        public SDModel(string name, string hash)
        {
            Name = name;
            Hash = hash;
        }
        public override string ToString() => $"{Name} ({Hash})";
    }
    public class SDLora
    {
        public string Name { get; init; }
        public string Hash { get; init; }
        public float Strength { get; init; }

        public SDLora(string name, string hash, float strength)
        {
            Name = name;
            Hash = hash;
            Strength = strength;
        }
        public override string ToString() => $"{Name} [{Strength}] ({Hash})";
    }
}
