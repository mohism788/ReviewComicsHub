using IssuesAPI.DTOs.ReviewsDTOs;
using IssuesAPI.Models;

namespace IssuesAPI.Repositories
{
    public interface IReviewRepository
    {
        //Get all reviews for a specific issue
        Task<IEnumerable<ReviewWithIssueTitleDto>> GetAllReviewsAsync(int issueId);
        //update review comment and/or rating
        Task<Review> UpdateReviewAsync(int reviewId,UpdatedReviewDto updatedReviewDto);
    }
}
