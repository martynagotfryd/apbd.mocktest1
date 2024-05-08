using apbd.mocktest1.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using apbd.mocktest1.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apbd.mocktest1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalsControlles : ControllerBase
    {
        private readonly IAnimalRepository _animalsRepository;

        public AnimalsControlles(IAnimalRepository animalsRepository)
        {
            _animalsRepository = animalsRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnimal(int id)
        {
            if (!await _animalsRepository.DoesAnimalExist(id))
            {
                return NotFound();
            }
            var animal = await _animalsRepository.GetAnimalById(id);
            return Ok(animal);
        }
    }
}
