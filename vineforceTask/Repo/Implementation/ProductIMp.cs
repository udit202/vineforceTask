using Microsoft.EntityFrameworkCore;
using vineforceTask.DatabaseConnect;
using vineforceTask.Models;
using vineforceTask.Repo.Interface;

namespace vineforceTask.Repo.Implementation
{
    public class ProductIMp : IProducts
    {
        private readonly ApplicationDbContext _context;

        public ProductIMp(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .OrderBy(p => p.Id)
                .ToListAsync();
        }
    }
}