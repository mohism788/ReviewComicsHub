using NewComicsAPI.DTOs;
using NewComicsAPI.Models;

namespace NewComicsAPI.Repositories
{
    public interface IComicsRepository
    {
        Task<IEnumerable<Comics>> GetAllComicsAsync(string? filterQuery = null, int pageSize = 30 ,int pageNumber = 1);
        Task<ComicWithIssuesDto> GetComicIssuesAsync(int comicId);
        Task<Comics> CreateComicAsync(Comics comic);
        Task<Comics> UpdateComicTitleAsync(int comicId, string newTitle);
        Task<bool> DeleteComicByIdAsync(int comicId);



    }
}
