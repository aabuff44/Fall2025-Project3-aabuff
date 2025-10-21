using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Fall2025_Project3_aabuff.Models;

namespace Fall2025_Project3_aabuff.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies{get; set;}
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Actor_Movie> Actor_Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Actor_Movie>()
                .HasKey(am => am.Id);

            modelBuilder.Entity<Actor_Movie>()
                .HasOne(am => am.Actor)
                .WithMany(a => a.Actor_Movies)
                .HasForeignKey(am => am.ActorId);

            modelBuilder.Entity<Actor_Movie>()
                .HasOne(am => am.Movie)
                .WithMany(m => m.Actor_Movies)
                .HasForeignKey(am => am.MovieId);

            // Prevent duplicate actor-movie relationships
            modelBuilder.Entity<Actor_Movie>()
                .HasIndex(am => new { am.ActorId, am.MovieId })
                .IsUnique();
        }
    }
}
