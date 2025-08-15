using System.ComponentModel.DataAnnotations.Schema;

namespace NewComicsAPI.DTOs
{
    [NotMapped]
    public class ReviewDto
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ReviewerName { get; set; }
        public DateTime Date { get; set; }
        public IssueDto Issue { get; set; } = null!; // Ensure Issue is not null
    }
}
