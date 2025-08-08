using ChaptersAPI.Models;
using IssuesAPI.DTOs.IssuesDTOs;

namespace IssuesAPI.Repositories
{
    public interface IIssuesRepositories
    {
        Task<IEnumerable<IssueDto>> GetAllIssuesAsync(int comicId, string filterQuery, int pageNumber, int pageSize);
        Task<IssueDto> UpdateIssueAsync(int issueId, UpdateIssueNameOrNumberDto updateIssueNameOrNumberDto);

        Task<bool> DeleteIssueByIdAsync(int issueId);

        //delete all issues linked with this comicId
        Task<bool> DeleteAllIssuesByComicIdAsync(int comicId);

        Task<CreateIssueDto> CreateIssueAsync(CreateIssueDto createIssueDto);
    }
}
