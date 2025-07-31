using System.ComponentModel.DataAnnotations;

namespace IssuesAPI.DTOs.ReviewsDTOs
{
    public class CreateReviewDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "IssueId must be a positive integer.")]
        public int IssueId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string Comment { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Reviewer name cannot exceed 100 characters.")]
        public string ReviewerName { get; set; }


        public DateTime Date { get; set; }
    }
}
