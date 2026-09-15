using System.ComponentModel.DataAnnotations;
using vineforceTask.Models;

namespace vineforceTask.DTO.order
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public OrderStatus Status { get; set; }
    }
}
