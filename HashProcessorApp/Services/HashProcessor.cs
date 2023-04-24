using System;
using System.Threading.Tasks;
using HashProcessorApp.Models;
using HashProcessorApp.Repositories;

namespace HashProcessorApp.Services
{
    public class HashProcessor
    {
        private readonly IHashRepository _hashRepository;

        public HashProcessor(IHashRepository hashRepository)
        {
            _hashRepository = hashRepository;
        }

        public async Task ProcessMessage(string hash)
        {
            var hashEntity = new Hash
            {
                Date = DateTime.UtcNow.Date,
                Sha1 = hash
            };

            await _hashRepository.AddHashAsync(hashEntity);
        }
    }
}
