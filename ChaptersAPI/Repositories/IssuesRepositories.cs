using System.Security.Claims;
using ChaptersAPI.Models;
using IssuesAPI.DataAccess;
using IssuesAPI.DTOs.IssuesDTOs;
using IssuesAPI.DTOs.ReviewsDTOs;
using IssuesAPI.Mapper.IssuesMapper;
using IssuesAPI.Mapper.ReviewsMapper;
using IssuesAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace IssuesAPI.Repositories
{
    public class IssuesRepositories : IIssuesRepositories
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IssuesRepositories(ApplicationDbContext dbContext, ILogger logger, IHttpContextAccessor  httpContextAccessor)
        {
            _dbContext = dbContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CreateIssueDto> CreateIssueAsync(CreateIssueDto createIssueDto)
        {
           if (createIssueDto != null)
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = user?.FindFirst(ClaimTypes.Role)?.Value;
                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

                var issue = IssuesMapper.MapToIssueFromCreate(createIssueDto);
                await _dbContext.Issues.AddAsync(issue);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"Issue with ID {issue.Id} created successfully.");
                return createIssueDto;
            }
            _logger.LogError("CreateIssueDto is null.");
            return null;

        }

        public async Task<bool> DeleteAllIssuesByComicIdAsync(int comicId)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var role = user?.FindFirst(ClaimTypes.Role)?.Value;
                var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

                var ToBeDeletedIssues = await _dbContext.Issues.Where(x => x.ComicId == comicId).ToListAsync();
                if (ToBeDeletedIssues == null || !ToBeDeletedIssues.Any())
                {
                    throw new Exception("No issues found for this comic.");
                }
                //delete all reviews linked to these issues
                var reviewsToDelete = await _dbContext.Reviews
                    .Where(x => ToBeDeletedIssues.Select(i => i.Id).Contains(x.IssueId))
                    .ToListAsync();
                if (reviewsToDelete != null && reviewsToDelete.Any())
                {
                    _dbContext.Reviews.RemoveRange(reviewsToDelete);
                    await _dbContext.SaveChangesAsync();
                }
                //delete the issues
                _dbContext.Issues.RemoveRange(ToBeDeletedIssues);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"All issues for comic with ID {comicId} deleted successfully.");
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        public async Task<bool> DeleteIssueByIdAsync(int issueId)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            var ToBeDeleted = await _dbContext.Issues.Where(x => x.Id == issueId).FirstOrDefaultAsync();
            if (ToBeDeleted == null)
            {
                throw new Exception("Issue not found");
            }
            //delete all reviews linked to this issue
            var reviewsToDelete = await _dbContext.Reviews.Where(x => x.IssueId == issueId).ToListAsync();
            if (reviewsToDelete != null && reviewsToDelete.Any())
            {
                _dbContext.Reviews.RemoveRange(reviewsToDelete);
                await _dbContext.SaveChangesAsync();
            }
            //delete the issue
            _dbContext.Issues.Remove(ToBeDeleted);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Issue with ID {issueId} deleted successfully.");
            return true;
        }

        public async Task<IEnumerable<IssueDto>> GetAllIssuesAsync(int comicId)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            var exist = await _dbContext.Issues.FirstOrDefaultAsync(x => x.ComicId == comicId);
            if (exist == null)
            {
                throw new Exception("Comic not found");
            }

            var issues = await _dbContext.Issues
                .Where(x => x.ComicId == comicId)
                .ToListAsync();

            //get ReviewWithIssueTitleDto for each issue
            var reviews = await _dbContext.Reviews
                .Where(x => issues.Select(i => i.Id).Contains(x.IssueId))
                .ToListAsync();

            var result = IssuesMapper.MapListToDtoWithReviews(issues, reviews);
            //log 
            _logger.LogInformation($"Found {result.Count()} issues for comic with ID {comicId}.");

            return result;
        }

        public async Task<IssueDto> UpdateIssueAsync(int issueId, UpdateIssueNameOrNumberDto updateIssueNameOrNumberDto)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();

            var exist = _dbContext.Issues.FirstOrDefault(x => x.Id == issueId);
            if (exist == null)
            {
                throw new Exception("Issue not found");
            }
            if (!string.IsNullOrEmpty(updateIssueNameOrNumberDto.IssueTitle) )
            {
                exist.IssueTitle = updateIssueNameOrNumberDto.IssueTitle;

            }
            if (updateIssueNameOrNumberDto.IssueNumber > 0)
            {
                exist.IssueNumber = updateIssueNameOrNumberDto.IssueNumber ??0;
            }
            _dbContext.Issues.Update(exist);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Issue with ID {exist.Id} updated successfully.");
            var result = IssuesMapper.MapUpdateIssueNameOrNumberDtoToIssueDto(exist.Id, updateIssueNameOrNumberDto);
            return result;
        }
    }
}
