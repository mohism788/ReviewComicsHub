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

        public Task<Comics> CreateComicAsync(Comics comic)
        {
            if (comic == null)
            {
                throw new ArgumentNullException(nameof(comic), "Comic cannot be null.");
            }
            _dbContext.Comics.Add(comic);
            _dbContext.SaveChanges();
            _logger.LogInformation($"Successfully created comic with ID {comic.ComicId} and title '{comic.Title}'.");
            return Task.FromResult(comic);

        }

        public async Task<bool> DeleteComicByIdAsync(int comicId)
        {
           var comicToDelete = await _dbContext.Comics.FirstOrDefaultAsync(x=> x.ComicId == comicId);
            if (comicToDelete == null)
            {
                return false;
            }
            // Delete all issues associated with this comic
            var issuesDeleted = await _issuesApiRepo.DeleteAllIssuesByComicIdAsync(comicId);
            if (!issuesDeleted)
            {
                _logger.LogWarning($"No issues found for comic with ID {comicId}. Proceeding to delete the comic without issues.");
            }
            // Delete the comic
            _dbContext.Comics.Remove(comicToDelete);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Successfully deleted comic with ID {comicId} and its associated issues.");
            return true;
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
