using ApiBlog.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBlog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        // Add Models
        public DbSet<Post> Post { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
