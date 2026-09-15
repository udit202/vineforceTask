using vineforceTask.DTO.order;
using vineforceTask.Models;

namespace vineforceTask.Repo.Interface
{
    public interface IOrder
    {
        Task<IEnumerable<Order>> GetAllAsync();

        Task<Order?> GetByIdAsync(int id);

        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);

        // Returns null if the referenced product doesn't exist or isn't active.
        Task<Order?> CreateAsync(CreateOrderDto dto);

        Task<Order?> UpdateStatusAsync(int id, OrderStatus status);

        Task<bool> DeleteAsync(int id);
    }

}
