using System.ComponentModel.DataAnnotations;

public class CreateOrderDto
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; } = 1;

    public string? CustomerName { get; set; }

    [EmailAddress]
    public string? CustomerEmail { get; set; }

    public string? CustomerPhone { get; set; }

    public string? ShippingAddress { get; set; }
}