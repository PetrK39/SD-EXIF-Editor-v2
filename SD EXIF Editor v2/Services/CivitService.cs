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
        private readonly ILoggingService _loggingService;

        public CivitService(MessageService messageService, ILoggingService loggingService)
        {
            _messageService = messageService;
            _loggingService = loggingService;

            client = new HttpClient();
            _loggingService.Trace("CivitService initialized.");
        }

        public async Task<CivitItem> GetItemFromHash(string origName, string origHash, float? strength = null)
        {
            _loggingService.Trace("Entering GetItemFromHash method.");
            _loggingService.Debug($"Requesting item with hash: {origHash}");

            var requestUri = $"https://civitai.com/api/v1/model-versions/by-hash/{origHash}";

            try
            {
                var response = await client.GetAsync(requestUri);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _loggingService.Error($"Failed to retrieve data from the API. Status Code: {response.StatusCode}, Content: {content}");
                    _messageService.ShowErrorMessage($"Failed to retrieve data from the API ({response.StatusCode})\r\n{content}");
                    return new CivitItem(origName, strength);
                }

                var data = JsonConvert.DeserializeObject<Root>(content);

                if (data is null || data.id == 0)
                {
                    _loggingService.Warn("Deserialized data is null or invalid.");
                    return new CivitItem(origName, strength);
                }

                var civitItem = new CivitItem(origName,
                    strength,
                    origName,
                    data.model.name,
                    data.model.type.ToUpper(),
                    data.files[0].sizeKB,
                    data.images.Select(i => new CivitItemImage(i.url, i.nsfwLevel)).ToList(),
                    data.downloadUrl,
                    $"https://civitai.com/models/{data.modelId}?modelVersionId={data.id}");

                _loggingService.Info($"Successfully retrieved and parsed item with hash: {origHash}");
                _loggingService.Trace("Exiting GetItemFromHash method.");
                return civitItem;
            }
            catch (HttpRequestException ex)
            {
                _loggingService.Error($"HttpRequestException while retrieving data from the API: {ex.Message}", ex);
                _messageService.ShowErrorMessage($"Failed to retrieve data from the API ({ex.StatusCode})\r\n{ex.Message}");
                return new CivitItem(origName, strength);
            }
            catch (JsonException ex)
            {
                _loggingService.Error($"JsonException while deserializing the API response: {ex.Message}", ex);
                _messageService.ShowErrorMessage($"Failed to deserialize the API response\r\n{ex.Message}");
                return new CivitItem(origName, strength);
            }
        }

        private sealed record Root(int id, int modelId, string name, Model model, List<File> files, List<Image> images, string downloadUrl);
        private sealed record Model(string name, string type);
        private sealed record File(double sizeKB);
        private sealed record Image(string url, NSFWLevels nsfwLevel);
    }
}
