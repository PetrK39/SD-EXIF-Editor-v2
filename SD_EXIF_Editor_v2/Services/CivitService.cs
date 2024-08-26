using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SD_EXIF_Editor_v2.Model;
using SD_EXIF_Editor_v2.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SD_EXIF_Editor_v2.Services
{
    public class CivitService : ICivitService
    {
        private readonly HttpClient client;
        private readonly IMessageService _messageService;
        private readonly ILogger<CivitService> _logger;

        public CivitService(IMessageService messageService, ILogger<CivitService> logger, HttpMessageHandler httpMessageHandler)
        {
            _messageService = messageService;
            _logger = logger;

            client = new HttpClient(httpMessageHandler);
            _logger.LogTrace("CivitService initialized.");
        }

        public async Task<CivitItem> GetItemFromHash(string origName, string origHash, float? strength = null)
        {
            _logger.LogTrace("Entering GetItemFromHash method.");
            _logger.LogDebug($"Requesting item with hash: {origHash}");

            var requestUri = $"https://civitai.com/api/v1/model-versions/by-hash/{origHash}";

            try
            {
                var response = await client.GetAsync(requestUri);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.NotFound:
                            _logger.LogError($"Failed to retrieve data from the API. Status Code: {response.StatusCode}, Content: {content}");
                            // No message here. That would happen if model wasn't uploaded to civit.ai
                            return new CivitItem(origName, strength);
                        default:
                            _logger.LogError($"Failed to retrieve data from the API. Status Code: {response.StatusCode}, Content: {content}");
                            await _messageService.ShowErrorMessageAsync($"Failed to retrieve data from the API ({response.StatusCode})\r\n{content}");
                            return new CivitItem(origName, strength);
                    }
                }

                var data = JsonConvert.DeserializeObject<Root>(content);

                if (data is null || data.id == 0)
                {
                    _logger.LogWarning("Deserialized data is null or invalid.");
                    return new CivitItem(origName, strength);
                }

                var civitItem = new CivitItem(origName,
                    strength,
                    data.model.name,
                    data.name,
                    data.model.type,
                    data.files[0].sizeKB,
                    data.images.Select(i => new CivitItemImage(i.url, i.nsfwLevel)).ToList(),
                    data.downloadUrl,
                    $"https://civitai.com/models/{data.modelId}?modelVersionId={data.id}");

                _logger.LogInformation($"Successfully retrieved and parsed item with hash: {origHash}");
                _logger.LogTrace("Exiting GetItemFromHash method.");
                return civitItem;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HttpRequestException while retrieving data from the API: {ex.Message}", ex);
                await _messageService.ShowErrorMessageAsync($"Failed to retrieve data from the API ({ex.StatusCode})\r\n{ex.Message}");
                return new CivitItem(origName, strength);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JsonException while deserializing the API response: {ex.Message}", ex);
                await _messageService.ShowErrorMessageAsync($"Failed to deserialize the API response\r\n{ex.Message}");
                return new CivitItem(origName, strength);
            }
        }

        private sealed record Root(int id, int modelId, string name, Model model, List<File> files, List<Image> images, string downloadUrl);
        private sealed record Model(string name, string type);
        private sealed record File(double sizeKB);
        private sealed record Image(string url, NSFWLevels nsfwLevel);
    }
}
