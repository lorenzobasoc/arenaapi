using Microsoft.EntityFrameworkCore;

namespace ArenaApi.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<FighterEntity> Fighters { get; set; }
    }
}
