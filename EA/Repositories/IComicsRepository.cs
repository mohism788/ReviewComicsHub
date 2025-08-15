using ComicsAPI_V2.DTOs;
using ComicsAPI_V2.Models;

namespace ComicsAPI_V2.Repositories
{
    
   public interface IComicsRepository
   {
       Task<IEnumerable<Comics>> GetAllComicsAsync(string? filterQuery = null, int pageSize = 30, int pageNumber = 1);
       Task<ComicWithIssuesDto> GetComicIssuesAsync(int comicId);
       Task<Comics> CreateComicAsync(Comics comic);
       Task<Comics> UpdateComicTitleAsync(int comicId, string newTitle);
       Task<bool> DeleteComicByIdAsync(int comicId);



   
   }
}
