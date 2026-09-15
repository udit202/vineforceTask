using Microsoft.AspNetCore.Mvc;
using vineforceTask.DTO;
using vineforceTask.DTO.order;
using vineforceTask.Repo.Implementation;
using vineforceTask.Repo.Interface;

namespace vineforceTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderCrud : ControllerBase
    {
        private readonly IOrder _orderRepository;

        public OrderCrud(IOrder orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // GET: api/OrderCrud
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderRepository.GetAllAsync();

            return Ok(orders);
        }

        // GET: api/OrderCrud/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Order not found."
                });
            }

            return Ok(order);
        }

        // GET: api/OrderCrud/user/USR-ABCDEF123456
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var orders = await _orderRepository.GetByUserIdAsync(userId);

            return Ok(orders);
        }

        // POST: api/OrderCrud
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _orderRepository.CreateAsync(dto);

            if (order == null)
            {
                return BadRequest(new
                {
                    message = "Could not create order. Check that the product exists and is active."
                });
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = order.Id },
                order);
        }

        // PUT: api/OrderCrud/1/status
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateOrderStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _orderRepository.UpdateStatusAsync(id, dto.Status);

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Order not found."
                });
            }

            return Ok(order);
        }

        // DELETE: api/OrderCrud/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _orderRepository.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new
                {
                    message = "Order not found."
                });
            }

            return Ok(new
            {
                message = "Order deleted successfully."
            });
        }
    }
}