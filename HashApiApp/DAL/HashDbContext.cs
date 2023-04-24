using Microsoft.EntityFrameworkCore;
using HashApiApp.Models;

namespace HashApiApp.DAL
{
    public class HashDbContext : DbContext
    {
        public HashDbContext(DbContextOptions<HashDbContext> options) : base(options)
        {
        }

        public DbSet<Hash> Hashes { get; set; }
    }
}
