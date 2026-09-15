namespace vineforceTask.Models
{
    public class Order
    {
        public int Id { get; set; }

        // Randomly generated public-facing user identifier (not the DB PK)
        public string UserId { get; set; } = Guid.NewGuid().ToString("N");

        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public int Quantity { get; set; } = 1;

        public decimal UnitPrice { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Shipping / contact info
        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public string CustomerPhone { get; set; } = string.Empty;

        public string ShippingAddress { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        // Navigation to payment record
        public Payment? Payment { get; set; }
    }
}
