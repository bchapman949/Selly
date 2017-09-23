using Microsoft.EntityFrameworkCore;

namespace Selly.NMS.Web.Models
{
    public class MainDbContext : DbContext
    {
        public DbSet<Device> Devices { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<PacketDroppedEvent> Events { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<PolicyRule> PolicyRules { get; set; }


        // TODO: DB Context: Not from DI
        public MainDbContext() { }

        public MainDbContext(DbContextOptions<MainDbContext> options) : base (options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Startup.Instance["Data:ConnectionStrings:ConnectionString"]);
        }
    }
}
