using Newtonsoft.Json;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System.Net.Http;

namespace SD_EXIF_Editor_v2.Service
{
    public class CivitService : ICivitService
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
                    return new(origName, strength);

                return new CivitItem(origName,
                    strength,
                    origName,
                    data.model.name,
                    data.model.type.ToUpper(),
                    data.files[0].sizeKB,
                    data.images.Select(i => new CivitItemImage(i.url, i.nsfwLevel)).ToList(),
                    data.downloadUrl,
                    $"https://civitai.com/models/{data.modelId}?modelVersionId={data.id}");
            }
            catch (HttpRequestException ex)
            {
                _messageService.ShowErrorMessage($"Failed to retrieve data from the API ({ex.StatusCode})\r\n{ex.Message}");
                return new(origName, strength);
            }
            catch (JsonException ex)
            {
                _messageService.ShowErrorMessage($"\"Failed to deserialize the API response\r\n{ex.Message}");
                return new(origName, strength);
            }
        }

        private sealed record Root(int id, int modelId, string name, Model model, List<File> files, List<Image> images, string downloadUrl);
        private sealed record Model(string name, string type);
        private sealed record File(double sizeKB);
        private sealed record Image(string url, NSFWLevels nsfwLevel);
    }
}
