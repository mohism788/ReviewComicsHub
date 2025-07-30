using IssuesAPI.DTOs.ReviewsDTOs;
using IssuesAPI.Models;

namespace IssuesAPI.Mapper.ReviewsMapper
{
    public class ReviewsMapper
    {
        //create a map from List of Reviews to ReviewWithIssueTitleDto
        public static IEnumerable<ReviewWithIssueTitleDto> MapToDtoReviewWithIssueTitle(IEnumerable<Review> reviews, string? issueTitle)
        {
            if (reviews == null || !reviews.Any())
            {
                return Enumerable.Empty<ReviewWithIssueTitleDto>();
            }
            return reviews.Select(review => new ReviewWithIssueTitleDto
            {
                Id = review.Id,
                IssueId = review.IssueId,
                Rating = review.Rating,
                Comment = review.Comment,
                ReviewerName = review.ReviewerName,
                Date = review.Date,
                IssueTitle = issueTitle ?? "Unknown Issue Title" // Fallback if issueTitle is null
            });
        }




    }
}
