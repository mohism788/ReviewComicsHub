namespace NewComicsAPI.DTOs
{
    public class UpdatedReviewDtoInComics
    {
        public int Id { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime Date { get; set; }
    }
}
