using Microsoft.EntityFrameworkCore;
using Filmotheque.Models;

namespace Filmotheque.Data
{
    public class APIContext : DbContext
    {
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }

        public APIContext(DbContextOptions<APIContext> options) : base(options)
        {

        }
    }
}
