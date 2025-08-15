namespace ComicsAPI.DTOs
{
    public class ComicWithIssuesDto
    {
        public int ComicId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? CoverImageUrl { get; set; }
        public IEnumerable<IssueDto> Issues { get; set; } = new List<IssueDto>();
    }
}
