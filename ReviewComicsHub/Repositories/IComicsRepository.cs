using ComicsAPI.DTOs;
using ComicsAPI.Models;

namespace ComicsAPI.Repositories
{
    public interface IComicsRepository
    {
        Task<IEnumerable<Comics>> GetAllComicsAsync();
        // Task<Comics> GetComicIssuesByIdAsync(int comicId);

        Task<ComicWithIssuesDto> GetComicIssuesAsync(int comicId);

        //update title only
        Task<Comics> UpdateComicTitleAsync(int comicId, string newTitle);

        
    }
}
