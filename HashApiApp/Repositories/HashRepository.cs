using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HashApiApp.DAL;
using HashApiApp.Models;

namespace HashApiApp.Repositories
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

        public async Task<List<HashGroupedByDate>> GetHashesGroupedByDateAsync()
        {
            return await _context.Hashes
                .GroupBy(h => h.Date)
                .Select(g => new HashGroupedByDate { Date = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

    }
}
