using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HashApiApp.Models;
using HashApiApp.Repositories;
using HashApiApp.RabbitMq;

namespace HashApiApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HashesController : ControllerBase
    {
        private readonly IHashRepository _hashRepository;
        private readonly RabbitMqProducer _rabbitMqProducer;

        public HashesController(IHashRepository hashRepository, RabbitMqProducer rabbitMqProducer)
        {
            _hashRepository = hashRepository;
            _rabbitMqProducer = rabbitMqProducer;
        }

        [HttpPost(Name = "/hashes")]
        public IActionResult GenerateHashes()
        {
            const int numberOfHashes = 40000;

            for (int i = 0; i < numberOfHashes; i++)
            {
                var randomSha1 = GenerateRandomSha1();

                // Send the hash to the RabbitMQ queue
                _rabbitMqProducer.SendHash(randomSha1);
            }

            return Ok();
        }

        [HttpGet(Name = "/hashes")]
        public async Task<ActionResult> GetHashesGroupedByDate()
        {
            var result = await _hashRepository.GetHashesGroupedByDateAsync();

            if (result.Count == 0)
            {
                return NotFound("No hashes found.");
            }

            var response = new
            {
                hashes = result.Select(h => new
                {
                    date = h.Date.ToString("yyyy-MM-dd"),
                    count = h.Count
                }).ToList()
            };

            return Ok(response);
        }

        private string GenerateRandomSha1()
        {
            using var sha1 = new System.Security.Cryptography.SHA1Managed();
            var randomBytes = new byte[16];
            new Random().NextBytes(randomBytes);
            var hashBytes = sha1.ComputeHash(randomBytes);
            return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
        }

    }
}
