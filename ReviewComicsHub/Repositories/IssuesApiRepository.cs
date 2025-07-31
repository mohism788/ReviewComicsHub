using System.Text.Json;
using System.Text;
using ComicsAPI.DTOs;

namespace ComicsAPI.Repositories
{
    public class IssuesApiRepository : IIssuesApiRepository
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ILogger _logger;

        public IssuesApiRepository(IHttpClientFactory httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<bool> DeleteAllIssuesByComicIdAsync(int comicId)
        {
            
                var payload = new { comicId };
                var client = _httpClient.CreateClient("IssuesApi");
                var response = await client.DeleteAsync($"https://localhost:7194/api/Issues/comic/{comicId}");
                if (!response.IsSuccessStatusCode)
                {
                //log failed or no issues found
                _logger.LogWarning($"Failed to delete issues for comic ID {comicId}: {response.ReasonPhrase}");
                return false;


            }
                // Log the response status code
                _logger.LogInformation($"Response from Issues API for comic ID {comicId}: {response.StatusCode}");
                return response.IsSuccessStatusCode;

        }

        public async Task<IEnumerable<IssueDto>> GetAllIssuesAsync(int comicId)
        {
            var payload = comicId;
            var client = _httpClient.CreateClient("IssuesApi");
            var response = await client.GetAsync($"https://localhost:7194/api/Issues/{comicId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error fetching issues for comic ID {comicId}: {response.ReasonPhrase}");
            }
            // Log the response status code
            _logger.LogInformation($"Response from Issues API for comic ID {comicId}: {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            var issues = JsonSerializer.Deserialize<IEnumerable<IssueDto>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return issues ?? Enumerable.Empty<IssueDto>();


        }
    }
}
