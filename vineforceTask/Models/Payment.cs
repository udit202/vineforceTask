using System.Text.Json.Serialization;

namespace vineforceTask.Models
{
    public class Payment
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        // Prevent:
        // Order -> Payment -> Order -> Payment -> ...
        [JsonIgnore]
        public Order? Order { get; set; }

        // Razorpay identifiers
        public string RazorpayOrderId { get; set; } = string.Empty;

        public string RazorpayPaymentId { get; set; } = string.Empty;

        public string RazorpaySignature { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "INR";

        public PaymentStatus Status { get; set; } = PaymentStatus.Created;

        // Razorpay payment method:
        // card, upi, netbanking, wallet, etc.
        public string PaymentMethod { get; set; } = string.Empty;

        // Raw webhook/response payload
        public string? RawResponse { get; set; }

        // Reason if payment failed
        public string? FailureReason { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}