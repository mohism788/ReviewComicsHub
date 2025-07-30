using ComicsAPI.DataAccess;
using ComicsAPI.DTOs;
using ComicsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicsAPI.Repositories
{
    public class ComicsRepository : IComicsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IIssuesApiRepository _issuesApiRepo;
        private readonly ILogger _logger;

        public ComicsRepository(ApplicationDbContext dbContext, IIssuesApiRepository issuesApiRepository, ILogger logger )
        {
            _dbContext = dbContext;
            _issuesApiRepo = issuesApiRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<Comics>> GetAllComicsAsync()
        {
            // Log the start of the method execution
            _logger.LogInformation("Fetching all comics from the database.");
            // Fetch all comics from the database
            _logger.LogInformation("Successfully fetched all comics from the database.");
            return await _dbContext.Comics.ToListAsync();
        }

        public async Task<ComicWithIssuesDto> GetComicIssuesAsync(int comicId)
        {

           var issues = await _issuesApiRepo.GetAllIssuesAsync(comicId);
            if (issues == null || !issues.Any())
            {
                throw new Exception($"No issues found for comic with ID {comicId}.");
            }
            _logger.LogInformation($"Found {issues.Count()} issues for comic with ID {comicId}.");

            var comic = await _dbContext.Comics
                .FirstOrDefaultAsync(c => c.ComicId == comicId);
            if (comic == null)
            {
                throw new Exception($"Comic with ID {comicId} not found.");
            }
            _logger.LogInformation($"Successfully retrieved comic with ID {comicId} and its issues.");

            var comicWithIssuesDto = Mapper.ComicMapper.MapToComicWithIssuesDto(comic, issues);
            return comicWithIssuesDto;

        }

        public async Task<Comics> UpdateComicTitleAsync(int comicId, string newTitle)
        {
            var exist = await _dbContext.Comics.FirstOrDefaultAsync(c => c.ComicId == comicId);
            if (exist == null)
            {
                throw new Exception($"Comic with ID {comicId} not found.");
            }
            exist.Title = newTitle;
            _dbContext.Comics.Update(exist);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Successfully updated title for comic with ID {comicId} to '{newTitle}'.");
            return exist;
        }
    }
}
