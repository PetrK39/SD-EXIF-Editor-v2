using Newtonsoft.Json;
using SD_EXIF_Editor_v2.Model;
using System.Net.Http;

namespace SD_EXIF_Editor_v2.Service
{
    public class CivitService
    {
        private readonly HttpClient client;
        private readonly MessageService _messageService;
        public CivitService(MessageService messageService)
        {
            _messageService = messageService;

            client = new HttpClient();
        }
        public async Task<CivitItem> GetItemFromHash(string origName, string origHash, float? strength = null)
        {
            var requestUri = $"https://civitai.com/api/v1/model-versions/by-hash/{origHash}";


            try
            {
                var response = await client.GetAsync(requestUri);
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Root>(content);

                if (data is null || data.id == 0)
                    return BuildDefaultCivitItem(origName, strength);

                return new CivitItem
                {
                    IsUnknown = false,

                    PromptName = origName,
                    Strength = strength,

                    OriginalName = data.model.name,
                    OriginalVersion = data.name,
                    Type = data.model.type.ToUpper(),

                    SizeKB = data.files[0].sizeKB,

                    Images = data.images.Select(i => new CivitItemImage { NSFWLevel = i.nsfwLevel, Uri = i.url }).ToList(),

                    DownloadUri = data.downloadUrl,
                    SiteUri = $"https://civitai.com/models/{data.modelId}?modelVersionId={data.id}"
                };
            }
            catch (HttpRequestException ex)
            {
                _messageService.ShowErrorMessage($"Failed to retrieve data from the API ({ex.StatusCode})\r\n{ex.Message}");
                return BuildDefaultCivitItem(origName, strength);
            }
            catch (JsonException ex)
            {
                _messageService.ShowErrorMessage($"\"Failed to deserialize the API response\r\n{ex.Message}");
                return BuildDefaultCivitItem(origName, strength);
            }
        }
        private CivitItem BuildDefaultCivitItem(string name, float? strength) => new() { IsUnknown = true, PromptName = name, Strength = strength };

        public record Root(int id, int modelId, string name, Model model, List<File> files, List<Image> images, string downloadUrl);
        public record Model(string name, string type);
        public record File(double sizeKB);
        public record Image(string url, NSFWLevels nsfwLevel);
    }
}
