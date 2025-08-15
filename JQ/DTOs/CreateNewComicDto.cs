using System.ComponentModel.DataAnnotations;

namespace ComicsAPI.DTOs
{
    public class CreateNewComicDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Publisher cannot exceed 500 characters.")]
        public string Description { get; set; }

        public string? CoverImageUrl { get; set; }
    }
}
