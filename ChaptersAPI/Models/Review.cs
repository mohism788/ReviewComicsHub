using ChaptersAPI.Models;
using Microsoft.AspNetCore.Components.Routing;

namespace IssuesAPI.Models
{
    public class Review
    {
        public int Id{ get; set; }
        public int IssueId{ get; set; }
        public int Rating{ get; set; }
        public string Comment{ get; set; }
        public string ReviewerName{ get; set; }
        public DateTime Date{ get; set; }
        //navigate to Issue
        public Issue Issue { get; set; } = null!; // Ensure Issue is not null

    }
}
