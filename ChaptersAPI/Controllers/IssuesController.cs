using IssuesAPI.DTOs.IssuesDTOs;
using IssuesAPI.Repositories;
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
    }
}
