using System.Reflection.Metadata.Ecma335;
using IssuesAPI.DataAccess;
using IssuesAPI.DTOs.ReviewsDTOs;
using IssuesAPI.Models;
using Microsoft.EntityFrameworkCore;
using IssuesAPI.Mapper.ReviewsMapper;
using System.Security.Claims;

namespace IssuesAPI.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReviewRepository(ApplicationDbContext dbContext, ILogger logger, IHttpContextAccessor httpContextAccessor)
        {
           
            _dbContext = dbContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateReviewDto> CreateReviewAsync(CreateReviewDto Createdreview)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            var username = user?.FindFirst(ClaimTypes.Name)?.Value
                                      ?? user?.FindFirst("unique_name")?.Value;


            //create a new review using the CreateReviewDto
            var review = ReviewsMapper.MapToReviewFromCreate(Createdreview, username);
            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();
            // Log the successful creation
            _logger.LogInformation($"Review for issue with ID {review.IssueId} created successfully.");
            return Createdreview;

        }

        public async Task<bool> DeleteAllReviewsByIssueIdAsync(int issueId)
        {

            try
            {
                var reviewsToDelete = await _dbContext.Reviews.Where(x => x.IssueId == issueId).ToListAsync();
                if (reviewsToDelete == null || !reviewsToDelete.Any())
                {
                    throw new Exception($"No reviews found for issue with ID {issueId}.");
                }
                _dbContext.Reviews.RemoveRange(reviewsToDelete);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        public async Task<bool> DeleteReviewByIdAsync(int reviewId)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            var username = user?.FindFirst(ClaimTypes.Name)?.Value
                                      ?? user?.FindFirst("unique_name")?.Value;
            try
            {
                var exist = await _dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);
                if (exist == null)
                {
                    throw new Exception($"Review with ID {reviewId} not found.");
                }
                if (exist.ReviewerName == username || role == "Moderator")
                {
                    _dbContext.Reviews.Remove(exist);
                    await _dbContext.SaveChangesAsync();
                    // Log the successful deletion
                    _logger.LogInformation($"Review with ID {reviewId} deleted successfully.");
                    return true;
                }
                else
                {
                    // Log the unauthorized deletion attempt
                    _logger.LogWarning($"User {username} attempted to delete a review they do not own (ID: {reviewId}).");
                    throw new UnauthorizedAccessException("You can only delete your own reviews.");
                }


            }
            catch (Exception)
            {
                return false;
                throw;
                
            }

        }

        public async Task<IEnumerable<ReviewWithIssueTitleDto>> GetAllReviewsAsync(int issueId)
        {
            //return list of reviews when found linked with this issueId
            
            var reviews = await _dbContext.Reviews
                .Where(x => x.IssueId == issueId)
                .ToListAsync();

            //get the issue title this these reviews are linked to
            var issueTitle = await _dbContext.Issues
                .Where(x => x.Id == issueId)
                .Select(x => x.IssueTitle)
                .FirstOrDefaultAsync();

            ////map the reviews to ReviewWithIssueTitleDto using Mapper
            var reviewsDto = ReviewsMapper.MapToDtoReviewWithIssueTitle(reviews,issueTitle);

            // Log the number of reviews found
            _logger.LogInformation($"Found {reviewsDto.Count()} reviews for issue with ID {issueId}.");

            if (reviewsDto == null || !reviewsDto.Any())
            {
                throw new Exception("No reviews found for this issue");
            }
            return reviewsDto;
        }

        public async Task<Review> UpdateReviewAsync(int reviewId, UpdatedReviewDto updatedReviewDto)
        {
            //use the UpdatedReviewDto
            var review = await _dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);
            if (review == null)
            {
                throw new Exception($"Review with ID {reviewId} not found.");
            }
            //update the comment and/or rating
            if (updatedReviewDto.Comment != null)
            {
                review.Comment = updatedReviewDto.Comment;
            }
            if (updatedReviewDto.Rating.HasValue)
            {
                review.Rating = updatedReviewDto.Rating.Value;
            }
            _dbContext.Reviews.Update(review);
            await _dbContext.SaveChangesAsync();
            // Log the successful update
            _logger.LogInformation($"Review with ID {reviewId} updated successfully.");
            return review;
        }
    }
}
