using ChaptersAPI.Models;

namespace IssuesAPI.DTOs.ReviewsDTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ReviewerName { get; set; }
        public DateTime Date { get; set; }
        public Issue Issue { get; set; } = null!; // Ensure Issue is not null
    }
}
