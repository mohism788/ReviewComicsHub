namespace ComicsAPI_V2.DTOs
{
    public class IssueDto
    {
        public int Id { get; set; }
        public int ComicId { get; set; }
        public int IssueNumber { get; set; }
        public string IssueTitle { get; set; }
        public ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();
    }
}
