using NewComicsAPI.DTOs;

namespace NewComicsAPI.Repositories
{
    public interface IIssuesApiRepository
    {
        //Return a list of issues for a specific comic
        Task<IEnumerable<IssueDto>> GetAllIssuesAsync(int comicId);

        //remove all issues linked with this comicId
        Task<bool> DeleteAllIssuesByComicIdAsync(int comicId);
    }
}
