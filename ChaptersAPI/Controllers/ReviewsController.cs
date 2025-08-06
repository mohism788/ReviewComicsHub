using IssuesAPI.DTOs.ReviewsDTOs;
using IssuesAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet]
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


        [Authorize(Roles = "Moderator")]
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


        [Authorize(Roles = "Moderator")]
        [HttpDelete]
        [Route("{reviewId:int}")]
        public async Task<IActionResult> DeleteReviewById([FromRoute] int reviewId)
        {
            var ToDelete = await _reviewRepo.DeleteReviewByIdAsync(reviewId);
            if (ToDelete)
            {
                return Ok($"Review with ID {reviewId} deleted successfully.");
            }
            return NotFound($"Review with ID {reviewId} not found.");
           
        }


        [Authorize(Roles = "Moderator")]
        [HttpDelete]
        [Route("issue/{issueId:int}")]
        public async Task<IActionResult> DeleteAllReviewsByIssueId([FromRoute] int issueId)
        {
            try
            {
                var deleted = await _reviewRepo.DeleteAllReviewsByIssueIdAsync(issueId);
                if (deleted)
                {
                    return Ok($"All reviews for issue with ID {issueId} deleted successfully.");
                }
                return NotFound($"No reviews found for issue with ID {issueId}.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto createReviewDto)
        {
            if (createReviewDto == null)
            {
                return BadRequest("Invalid review data.");
            }
            try
            {
                var createdReview = await _reviewRepo.CreateReviewAsync(createReviewDto);
                return CreatedAtAction(nameof(GetAllReviews), new { issueId = createdReview.IssueId }, createdReview);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }


    }
}
