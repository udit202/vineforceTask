using vineforceTask.Models;

namespace vineforceTask.Repo.Interface
{
    public interface IProducts
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}