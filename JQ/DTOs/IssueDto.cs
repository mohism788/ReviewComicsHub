using System.ComponentModel.DataAnnotations.Schema;

namespace ComicsAPI.DTOs
{
    [NotMapped]
    public class IssueDto
    {
        public int Id { get; set; }
        public int ComicId { get; set; }
        public int IssueNumber { get; set; }
        public string IssueTitle { get; set; }
        public ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    }
}
