using Microsoft.EntityFrameworkCore;
using Filmotheque.Models;

namespace Filmotheque.Data
{
    public class APIContext : DbContext
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Movie> Movies { get; set; }

        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>().HasMany(m => m.Actors).WithMany();
            modelBuilder.Entity<Movie>().HasMany(m => m.Directors).WithMany();
        }
    }
}
