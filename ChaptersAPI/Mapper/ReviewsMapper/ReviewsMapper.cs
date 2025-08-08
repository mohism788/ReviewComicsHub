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


        //map CreateReviewDto to Review
        public static Review MapToReviewFromCreate(CreateReviewDto createReviewDto, string username)
        {
            if (createReviewDto == null)
            {
                throw new ArgumentNullException(nameof(createReviewDto), "CreateReviewDto cannot be null");
            }
            return new Review
            {

                IssueId = createReviewDto.IssueId,
                Rating = createReviewDto.Rating,
                Comment = createReviewDto.Comment,
                ReviewerName = username,
                Date = DateTime.UtcNow // Set the date to the current UTC time
            };
        }

    }
}
