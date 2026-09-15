using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vineforceTask.DTO;
using vineforceTask.Models;
using vineforceTask.Repo.Interface;

namespace vineforceTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryCrud : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;

        public CountryCrud(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        // GET: api/CountryCrud
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var countries = await _countryRepository.GetAllAsync();

            return Ok(countries);
        }

        // GET: api/CountryCrud/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var country = await _countryRepository.GetByIdAsync(id);

            if (country == null)
            {
                return NotFound(new
                {
                    message = "Country not found."
                });
            }

            return Ok(country);
        }

        // POST: api/CountryCrud
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateCountryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var country = await _countryRepository.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = country.Id },
                country);
        }

        // PUT: api/CountryCrud/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateCountryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var country = await _countryRepository.UpdateAsync(id, dto);

            if (country == null)
            {
                return NotFound(new
                {
                    message = "Country not found."
                });
            }

            return Ok(country);
        }

        // DELETE: api/CountryCrud/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _countryRepository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Country not found."
                });
            }

            return Ok(new
            {
                message = "Country deleted successfully."
            });
        }
    }
}