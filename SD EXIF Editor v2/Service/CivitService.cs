using Newtonsoft.Json;
using RestSharp;
using SD_EXIF_Editor_v2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Service
{
    public class CivitService
    {
        private readonly RestClient client;
        public CivitService()
        {
            client = new RestClient("https://civitai.com/api/v1");
        }
        public async Task<CivitItem> GetItemFromHash(string origName, string origHash, float? strength = null)
        {
            var request = new RestRequest("model-versions/by-hash/" + origHash);

            var response = await client.GetAsync(request);
            var data = JsonConvert.DeserializeObject<Root>(response.Content);

            if (data.id == 0)
                return new CivitItem
                {
                    IsUnknown = true,

                    PromptName = origName,
                    Strength = strength
                };
            else
                return new CivitItem
                {
                    IsUnknown = false,

                    PromptName = origName,
                    Strength = strength,

                    OriginalName = data.model.name,
                    OriginalVersion = data.name,
                    Type = data.model.type.ToUpper(),

                    SizeKB = data.files[0].sizeKB,

                    Images = data.images.Select(i => new CivitItemImage { NSFWLevel = i.nsfwLevel, Uri = i.url}).ToList(),

                    DownloadUri = data.downloadUrl,
                    SiteUri = $"https://civitai.com/models/{data.modelId}?modelVersionId={data.id}"
                };
        }

        public class File
        {
            public double sizeKB { get; set; }
        }

        public class Image
        {
            public string url { get; set; }
            public NSFWLevels nsfwLevel { get; set; }
        }
        public class Model
        {
            public string name { get; set; }
            public string type { get; set; }
        }

        public class Root
        {
            public int id { get; set; }
            public int modelId { get; set; }
            public string name { get; set; }
            public Model model { get; set; }
            public List<File> files { get; set; }
            public List<Image> images { get; set; }
            public string downloadUrl { get; set; }
        }


    }
}
