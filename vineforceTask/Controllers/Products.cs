using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vineforceTask.Repo.Interface;

namespace vineforceTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Products : ControllerBase
    {
        private readonly IProducts _products;

        public Products(IProducts products)
        {
            _products = products;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _products.GetAllProductsAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while fetching products.",
                    error = ex.Message
                });
            }
        }
    }
}