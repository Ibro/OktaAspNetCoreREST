using Microsoft.EntityFrameworkCore;

namespace Api
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                  : base(options)
        { }

        public DbSet<JournalLog> JournalLogs { get; set; }
    }

}