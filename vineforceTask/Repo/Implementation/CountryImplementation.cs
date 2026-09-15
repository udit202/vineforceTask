using Microsoft.EntityFrameworkCore;
using vineforceTask.DatabaseConnect;
using vineforceTask.DTO;
using vineforceTask.Models;
using vineforceTask.Repo.Interface;

namespace vineforceTask.Repo.Implementation
{
    public class CountryImplementation : ICountryRepository
    {
        private readonly ApplicationDbContext _context;

        public CountryImplementation(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET ALL
        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            try
            {
                return await _context.Countries
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET BY ID
        public async Task<Country?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Countries
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // CREATE
        public async Task<Country> CreateAsync(CreateCountryDto dto)
        {
            try
            {
                var country = new Country
                {
                    Name = dto.Name,
                    Code = dto.Code,
                    IsActive = dto.IsActive,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Countries.Add(country);

                await _context.SaveChangesAsync();

                return country;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // UPDATE
        public async Task<Country?> UpdateAsync(
            int id,
            UpdateCountryDto dto)
        {
            try
            {
                var country = await _context.Countries
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (country == null)
                {
                    return null;
                }

                country.Name = dto.Name;
                country.Code = dto.Code;
                country.IsActive = dto.IsActive;
                country.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return country;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var country = await _context.Countries
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (country == null)
                {
                    return false;
                }

                _context.Countries.Remove(country);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}