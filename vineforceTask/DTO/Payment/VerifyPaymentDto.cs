using System.ComponentModel.DataAnnotations;

namespace vineforceTask.DTO.Payment
{
    public class VerifyPaymentDto
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public string RazorpayOrderId { get; set; } = string.Empty;

        [Required]
        public string RazorpayPaymentId { get; set; } = string.Empty;

        [Required]
        public string RazorpaySignature { get; set; } = string.Empty;

        // Method Razorpay reports back (card, upi, netbanking, wallet, etc.)
        public string? PaymentMethod { get; set; }
    }
}
