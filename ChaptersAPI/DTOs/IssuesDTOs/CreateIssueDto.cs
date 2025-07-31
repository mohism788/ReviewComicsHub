using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace IssuesAPI.DTOs.IssuesDTOs
{
    public class CreateIssueDto
    {
        [Required]
        public int ComicId { get; set; }

        [Required]
        public int IssueNumber { get; set; }

        [Required]
        public string IssueTitle { get; set; } 
    }
}
