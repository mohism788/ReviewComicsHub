using System.Text.Json;
using ComicsAPI.DTOs;

namespace ComicsAPI.Repositories
{
    public class ReviewApiRepository : IReviewApiRepository
    {
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClient;

        public ReviewApiRepository(ILogger logger, IHttpClientFactory httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

       /* public async Task<ReviewDto> UpdateReviewAsync(int reviewId, UpdatedReviewDtoInComics updatedReviewDto)
        {
            var payload = new UpdatedReviewDtoInComics
            {
                Comment = updatedReviewDto.Comment,
                Rating = updatedReviewDto.Rating
            };
            var client = _httpClient.CreateClient("ReviewsApi");
            var response = await client.PutAsJsonAsync($"https://localhost:7194/api/Reviews/{reviewId}", payload);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Error updating review with ID {reviewId}: {response.ReasonPhrase}");
                throw new Exception($"Error updating review with ID {reviewId}: {response.ReasonPhrase}");
            }
            // Log the response status code
            _logger.LogInformation($"Response from Reviews API for review ID {reviewId}: {response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            var updatedReview = JsonSerializer.Deserialize<ReviewDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (updatedReview == null)
            {
                _logger.LogError($"No review found with ID {reviewId}.");
                throw new Exception($"No review found with ID {reviewId}.");
            }
            _logger.LogInformation($"Successfully updated review with ID {reviewId}.");
            return updatedReview;*/


        
    }
}
