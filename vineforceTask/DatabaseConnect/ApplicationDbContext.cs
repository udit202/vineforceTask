using Microsoft.EntityFrameworkCore;
using vineforceTask.Models;

namespace vineforceTask.DatabaseConnect
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
    }
}