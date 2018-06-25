using Microsoft.EntityFrameworkCore;
 
namespace weddingPlanner.Models
{
    public class LoginContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public LoginContext(DbContextOptions<LoginContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Wedding> weddings { get; set; }
        public DbSet<Guest> guest { get; set; }
    }
}