namespace ComicsAPI_V2.DTOs
{
    public class UpdateReviewDtoInComics
    {
        public int Id { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
