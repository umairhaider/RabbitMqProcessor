using System.Threading.Tasks;
using HashProcessorApp.Models;

namespace HashProcessorApp.Repositories
{
    public interface IHashRepository
    {
        Task AddHashAsync(Hash hash);
    }
}
