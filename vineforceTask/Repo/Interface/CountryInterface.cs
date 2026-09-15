using vineforceTask.DTO;
using vineforceTask.Models;

namespace vineforceTask.Repo.Interface
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetAllAsync();

        Task<Country?> GetByIdAsync(int id);

        Task<Country> CreateAsync(CreateCountryDto dto);

        Task<Country?> UpdateAsync(int id, UpdateCountryDto dto);

        Task<bool> DeleteAsync(int id);
    }
}