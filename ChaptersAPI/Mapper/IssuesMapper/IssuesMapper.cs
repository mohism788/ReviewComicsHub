using ChaptersAPI.Models;
using IssuesAPI.DataAccess;
using IssuesAPI.DTOs.IssuesDTOs;
using IssuesAPI.DTOs.ReviewsDTOs;
using IssuesAPI.Models;

namespace IssuesAPI.Mapper.IssuesMapper
{
    public class IssuesMapper
    {
       
        // Create a map from List of Issues to IssueWithReviewsDto
        public static IEnumerable<IssueDto> MapListToDtoWithReviews(IEnumerable<Issue> issues, List<Review> reviews)
        {
            if (issues == null || !issues.Any())
            {
                return Enumerable.Empty<IssueDto>();
            }
            return issues.Select(issue => new IssueDto
            {
                Id = issue.Id,
                ComicId = issue.ComicId,
                IssueNumber = issue.IssueNumber,
                IssueTitle = issue.IssueTitle,
                Reviews = reviews
                    .Where(review => review.IssueId == issue.Id)
                    .Select(review => new ReviewDto
                    {
                        Id = review.Id,
                        IssueId = review.IssueId,
                        Rating = review.Rating,
                        Comment = review.Comment,
                        ReviewerName = review.ReviewerName,
                        Date = review.Date,
                    }).ToList()
            });
        }


        //make a mapper from UpdateIssueNameOrNumberDto to IssueDto
        public static IssueDto MapUpdateIssueNameOrNumberDtoToIssueDto(int issueId, UpdateIssueNameOrNumberDto updateIssueNameOrNumberDto)
        {
            if (updateIssueNameOrNumberDto == null)
            {
                throw new ArgumentNullException(nameof(updateIssueNameOrNumberDto), "UpdateIssueNameOrNumberDto cannot be null");
            }
            
            return new IssueDto
            {
                Id = issueId,
                IssueTitle = updateIssueNameOrNumberDto.IssueTitle,
                IssueNumber = updateIssueNameOrNumberDto.IssueNumber ?? 0
            };
        }


        // // Map CreateIssueDto to Issue
        public static Issue MapToIssueFromCreate(CreateIssueDto createIssueDto)
        {
            if (createIssueDto == null)
            {
                throw new ArgumentNullException(nameof(createIssueDto), "CreateIssueDto cannot be null");
            }
            return new Issue
            {
                ComicId = createIssueDto.ComicId,
                IssueNumber = createIssueDto.IssueNumber,
                IssueTitle = createIssueDto.IssueTitle
            };


        }
    }
}
