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

        public IssuesRepositories(ApplicationDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<IEnumerable<IssueDto>> GetAllIssuesAsync(int comicId)
        {
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
