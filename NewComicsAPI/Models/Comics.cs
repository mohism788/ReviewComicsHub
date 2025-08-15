using NewComicsAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace NewComicsAPI.Models
{
    public class Comics
    {
        //make a primary key named ComicId
        public int ComicId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? CoverImageUrl{ get; set; }
        public ICollection<IssueDto> Issues { get; set; } = new List<IssueDto>();
    }

}
