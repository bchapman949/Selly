using Microsoft.EntityFrameworkCore;

namespace Selly.Agent.Windows.Data
{
    class DatabaseContext : DbContext
    {
        public DbSet<Config> Configuration { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: Hard coded path
            optionsBuilder.UseSqlite("Data Source=C:\\Selly\\Selly.db");
        }
    }
}
