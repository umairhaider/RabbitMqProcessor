using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HashApiApp.Models;

namespace HashApiApp.Repositories
{
    public interface IHashRepository
    {
        Task AddHashAsync(Hash hash);
        Task<List<HashGroupedByDate>> GetHashesGroupedByDateAsync();
    }
}
