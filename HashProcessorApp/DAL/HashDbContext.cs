using Microsoft.EntityFrameworkCore;
using HashProcessorApp.Models;


namespace HashProcessorApp.DAL
{
    public class HashDbContext : DbContext
    {
        public HashDbContext(DbContextOptions<HashDbContext> options) : base(options)
        {
        }

        public DbSet<Hash> Hashes { get; set; }
    }
}
