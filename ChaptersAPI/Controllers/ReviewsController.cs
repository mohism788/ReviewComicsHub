using IssuesAPI.DTOs.ReviewsDTOs;
using IssuesAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IssuesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepo;

        public ReviewsController(IReviewRepository reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        [HttpGet]
        //route for getting reviews by issueId
        [Route("{issueId:int}")]
        public async Task<IActionResult> GetAllReviews([FromRoute]int issueId)
        {
            try
            {
                var reviewsDomain = await _reviewRepo.GetAllReviewsAsync(issueId);
                if (reviewsDomain == null || !reviewsDomain.Any())
                {
                    return NotFound($"No reviews found for issue with ID {issueId}.");
                }
                return Ok(reviewsDomain);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{reviewId:int}")]
        public async Task<IActionResult> UpdateReview([FromRoute] int reviewId, [FromBody] UpdatedReviewDto updatedReviewDto)
        {
            try
            {
                var updatedReview = await _reviewRepo.UpdateReviewAsync(reviewId, updatedReviewDto);
                if (updatedReview == null)
                {
                    return NotFound($"Review with ID {reviewId} not found.");
                }
                return Ok(updatedReview);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
