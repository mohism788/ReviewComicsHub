using ChaptersAPI.Models;

namespace IssuesAPI.DTOs.ReviewsDTOs
{
    public class ReviewWithIssueTitleDto
    {
        public int Id { get; set; }
        public int IssueId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string ReviewerName { get; set; }
        public DateTime Date { get; set; }
        public string IssueTitle { get; set; } = null!; // Ensure IssueTitle is not null
    }
}
