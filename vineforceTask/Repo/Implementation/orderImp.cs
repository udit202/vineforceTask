using Microsoft.EntityFrameworkCore;
using vineforceTask.DatabaseConnect;
using vineforceTask.DTO.order;
using vineforceTask.Models;
using vineforceTask.Repo.Interface;

namespace vineforceTask.Repo.Implementation
{
    public class OrderRepository : IOrder
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(
            ApplicationDbContext context,
            ILogger<OrderRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.Payment)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.Payment)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.Product)
                .Include(o => o.Payment)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.Id)
                .ToListAsync();
        }

        public async Task<Order?> CreateAsync(CreateOrderDto dto)
        {
            try
            {
                // Get active product
                var product = await _context.Products
                    .FirstOrDefaultAsync(
                        p => p.Id == dto.ProductId && p.IsActive
                    );

                if (product == null)
                {
                    _logger.LogWarning(
                        "Create order failed: product {ProductId} not found or inactive.",
                        dto.ProductId
                    );

                    return null;
                }

                // Static customer details
                const string defaultCustomerName = "Udit Dhiman";
                const string defaultCustomerEmail = "uditdhiman1212@gmail.com";
                const string defaultCustomerPhone = "8685873433";
                const string defaultShippingAddress = "Jind, Haryana, India";

                var order = new Order
                {
                    // Random public-facing user identifier
                    UserId = $"USR-{Guid.NewGuid():N}"
                        .Substring(0, 16)
                        .ToUpper(),

                    ProductId = product.Id,
                    Quantity = dto.Quantity,

                    UnitPrice = product.Price,
                    TotalAmount = product.Price * dto.Quantity,

                    Status = OrderStatus.Pending,

                    // Use DTO value if provided,
                    // otherwise use static customer details.
                    CustomerName = string.IsNullOrWhiteSpace(dto.CustomerName)
                        ? defaultCustomerName
                        : dto.CustomerName,

                    CustomerEmail = string.IsNullOrWhiteSpace(dto.CustomerEmail)
                        ? defaultCustomerEmail
                        : dto.CustomerEmail,

                    CustomerPhone = string.IsNullOrWhiteSpace(dto.CustomerPhone)
                        ? defaultCustomerPhone
                        : dto.CustomerPhone,

                    ShippingAddress = string.IsNullOrWhiteSpace(dto.ShippingAddress)
                        ? defaultShippingAddress
                        : dto.ShippingAddress,

                    CreatedAt = DateTime.UtcNow
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Order {OrderId} created successfully for product {ProductId}.",
                    order.Id,
                    product.Id
                );

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating order for product {ProductId}.",
                    dto.ProductId
                );

                return null;
            }
        }

        public async Task<Order?> UpdateStatusAsync(
            int id,
            OrderStatus status)
        {
            try
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    return null;
                }

                order.Status = status;
                order.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating order {OrderId} status.",
                    id
                );

                return null;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (order == null)
                {
                    return false;
                }

                _context.Orders.Remove(order);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error deleting order {OrderId}.",
                    id
                );

                return false;
            }
        }
    }
}