using vineforceTask.DTO.Payment;
using vineforceTask.Models;

namespace vineforceTask.Repo.Interface
{
    public interface IPayment
    {
        // ============================================================
        // GET PAYMENT BY ID
        // ============================================================
        Task<Payment?> GetByIdAsync(int id);

        // ============================================================
        // GET PAYMENT BY ORDER ID
        // ============================================================
        Task<Payment?> GetByOrderIdAsync(int orderId);

        // ============================================================
        // CREATE PAYMENT
        // ============================================================
        // Creates the Payment row for an order and registers
        // the order with Razorpay to obtain a real RazorpayOrderId.
        //
        // Returns null if:
        // - Order doesn't exist
        // - Razorpay configuration is missing
        // - Razorpay order creation fails
        Task<Payment?> CreateAsync(
            CreatePaymentDto dto);

        // ============================================================
        // VERIFY PAYMENT
        // ============================================================
        // Verifies Razorpay signature.
        //
        // Successful:
        // - Payment status = Captured
        // - Linked order status = Paid
        //
        // Invalid signature:
        // - Payment status = Failed
        //
        // Returns null if payment/order is not found.
        Task<Payment?> VerifyAsync(
            VerifyPaymentDto dto);

        // ============================================================
        // UPDATE PAYMENT STATUS
        // ============================================================
        Task<Payment?> UpdateStatusAsync(
            int id,
            UpdatePaymentStatusDto dto);
    }
}