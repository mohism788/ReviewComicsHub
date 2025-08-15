using NewComicsAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace NewComicsAPI.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Comics> Comics { get; set; }
        public DbSet<Image> Images { get; set; }
        //assign ComicId as the primary key for the Comics table
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comics>()
                .HasKey(c => c.ComicId);
            modelBuilder.Entity<Comics>()
                .Property(c => c.Title)
                .IsRequired()
                .HasMaxLength(200);
            modelBuilder.Entity<Comics>()
                .Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(1000);
            modelBuilder.Entity<Comics>()
                .Property(c => c.CoverImageUrl)
                .HasMaxLength(500);
        }
        /* protected override void OnModelCreating(ModelBuilder modelBuilder)
         {

             modelBuilder.Entity<Comics>().ToTable("Comics");
             modelBuilder.Entity<Comics>().HasKey(c => c.Id);
             modelBuilder.Entity<Comics>().Property(c => c.Title).IsRequired().HasMaxLength(200);
             modelBuilder.Entity<Comics>().Property(c => c.Description).IsRequired().HasMaxLength(1000);
             modelBuilder.Entity<Comics>().Property(c => c.CoverImageUrl).HasMaxLength(500);

         }*/



    }
}
