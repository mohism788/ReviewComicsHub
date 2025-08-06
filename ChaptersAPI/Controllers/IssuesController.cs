using IssuesAPI.DTOs.IssuesDTOs;
using IssuesAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChaptersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        //integrate repo in constructor

        private readonly IIssuesRepositories _issuesRepo;
        public IssuesController(IIssuesRepositories issuesRepositories)
        {
            _issuesRepo = issuesRepositories;
        }

        [HttpGet("{comicId}")]
        [Authorize]
        public async Task<IActionResult> GetAllIssues(int comicId)
        {
            try
            {
                var issuesDomain = await _issuesRepo.GetAllIssuesAsync(comicId);
                if (issuesDomain == null || !issuesDomain.Any())
                {
                    return NotFound($"No issues found for comic with ID {comicId}.");
                }
                return Ok(issuesDomain);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        [Route("{issueId}")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> UpdateIssue([FromRoute] int issueId, [FromBody] UpdateIssueNameOrNumberDto updateIssueNameOrNumberDto)
        {
            try
            {
                var updatedIssue = await _issuesRepo.UpdateIssueAsync(issueId, updateIssueNameOrNumberDto);
                if (updatedIssue == null)
                {
                    return NotFound($"Issue with ID {issueId} not found.");
                }
                return Ok(updatedIssue);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{issueId}")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteIssueById([FromRoute] int issueId)
        {
            try
            {
                var deleted = await _issuesRepo.DeleteIssueByIdAsync(issueId);
                if (!deleted)
                {
                    return NotFound($"Issue with ID {issueId} not found.");
                }
                return Ok($"Issue with ID {issueId} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("comic/{comicId}")]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteAllIssuesByComicId([FromRoute] int comicId)
        {
            try
            {
                var deleted = await _issuesRepo.DeleteAllIssuesByComicIdAsync(comicId);
                if (!deleted)
                {
                    return NotFound($"No issues found for comic with ID {comicId}.");
                }
                return Ok($"All issues for comic with ID {comicId} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]

        public async Task<IActionResult> CreateNewIssue([FromBody] CreateIssueDto createIssueDto)
        {
            try
            {
                if (createIssueDto == null)
                {
                    return BadRequest("CreateIssueDto cannot be null.");
                }
                var createdIssue = await _issuesRepo.CreateIssueAsync(createIssueDto);
                if (createdIssue == null)
                { 
                    return BadRequest("Failed to create issue.");
                }
                return CreatedAtAction(nameof(GetAllIssues), new { comicId = createdIssue.ComicId }, createdIssue);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

        }




    }
}
