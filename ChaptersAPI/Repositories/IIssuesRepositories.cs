using ChaptersAPI.Models;
using IssuesAPI.DTOs.IssuesDTOs;

namespace IssuesAPI.Repositories
{
    public interface IIssuesRepositories
    {
        Task<IEnumerable<IssueDto>> GetAllIssuesAsync(int comicId);
        Task<IssueDto> UpdateIssueAsync(int issueId, UpdateIssueNameOrNumberDto updateIssueNameOrNumberDto);

    }
}
