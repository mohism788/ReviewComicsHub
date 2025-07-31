using IssuesAPI.DTOs.ReviewsDTOs;
using IssuesAPI.Models;

namespace IssuesAPI.Repositories
{
    public interface IReviewRepository
    {
        //Get all reviews for a specific issue
        Task<IEnumerable<ReviewWithIssueTitleDto>> GetAllReviewsAsync(int issueId);
        Task<Review> UpdateReviewAsync(int reviewId,UpdatedReviewDto updatedReviewDto);

        Task<bool> DeleteReviewByIdAsync(int reviewId);

        //delete all reviews linked with this issueId
        Task<bool> DeleteAllReviewsByIssueIdAsync(int issueId);

        Task<CreateReviewDto> CreateReviewAsync(CreateReviewDto Createdreview);

    }
}
