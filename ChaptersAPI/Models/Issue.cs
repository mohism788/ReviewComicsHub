using IssuesAPI.DTOs.ReviewsDTOs;
using IssuesAPI.Models;

namespace ChaptersAPI.Models
{
    public class Issue
    {
        public  int Id { get; set; }
        public  int ComicId{ get; set; }
        public  int IssueNumber{ get; set; }
        public  string IssueTitle{ get; set; }

        //navigate Review
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
