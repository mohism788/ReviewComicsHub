using ChaptersAPI.Models;
using IssuesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace IssuesAPI.DataAccess
{
    public class ApplicationDbContext :  DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Issue> Issues { get; set; }


        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>().ToTable("Reviews");
            modelBuilder.Entity<Issue>().ToTable("Issues");
        }*/
    }
}
