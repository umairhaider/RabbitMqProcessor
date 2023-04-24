using System.Threading.Tasks;
using HashProcessorApp.DAL;
using HashProcessorApp.Models;

namespace HashProcessorApp.Repositories
{
    public class HashRepository : IHashRepository
    {
        private readonly HashDbContext _context;

        public HashRepository(HashDbContext context)
        {
            _context = context;
        }

        public async Task AddHashAsync(Hash hash)
        {
            await _context.Hashes.AddAsync(hash);
            await _context.SaveChangesAsync();
        }
    }
}
