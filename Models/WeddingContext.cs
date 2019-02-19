using Microsoft.EntityFrameworkCore;
 
namespace WeddingPlanner.Models
{
    public class WeddingContext : DbContext
    {
        public WeddingContext(DbContextOptions<WeddingContext> options) : base(options) { }
        public DbSet<User> user {get;set;}
        public DbSet<Wedding> wedding {get;set;}
        public DbSet<Guests> guests {get;set;}
    }
}