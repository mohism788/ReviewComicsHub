using System.Reflection.Metadata.Ecma335;
using IssuesAPI.DataAccess;
using IssuesAPI.DTOs.ReviewsDTOs;
using IssuesAPI.Models;
using Microsoft.EntityFrameworkCore;
using IssuesAPI.Mapper.ReviewsMapper;
using System.Security.Claims;
using IssuesAPI.Helpers;

namespace IssuesAPI.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly JwtReceiver _jwtReceiver;

        public ReviewRepository(ApplicationDbContext dbContext, ILogger logger, JwtReceiver jwtReceiver)
        {
           
            _dbContext = dbContext;
            _logger = logger;
            _jwtReceiver = jwtReceiver;
        }

        public async Task<CreateReviewDto> CreateReviewAsync(CreateReviewDto Createdreview)
        {
            //create a new review using the CreateReviewDto
            var review = ReviewsMapper.MapToReviewFromCreate(Createdreview, _jwtReceiver.Username);
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
                // Check only Moderator is authorized to delete these reviews
                if (_jwtReceiver.Role != "Moderator")
                {
                    // Log the unauthorized deletion attempt
                    _logger.LogWarning($"User {_jwtReceiver.Username} attempted to delete reviews for issue {issueId} without proper authorization.");
                    throw new UnauthorizedAccessException("Only moderators can delete all reviews for an issue.");
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
            
            try
            {
                var exist = await _dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == reviewId);
                if (exist == null)
                {
                    throw new Exception($"Review with ID {reviewId} not found.");
                }
                if (exist.ReviewerName == _jwtReceiver.Username || _jwtReceiver.Role== "Moderator")
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
                    _logger.LogWarning($"User {_jwtReceiver.Username} attempted to delete a review they do not own (ID: {reviewId}).");
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
            // Check if the user is authorized to update the review
            if (review.ReviewerName != _jwtReceiver.Username && _jwtReceiver.Role != "Moderator")
            {
                // Log the unauthorized update attempt
                _logger.LogWarning($"User {_jwtReceiver.Username} attempted to update a review they do not own (ID: {reviewId}).");
                throw new UnauthorizedAccessException("You can only update your own reviews.");
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
