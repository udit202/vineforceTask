using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using vineforceTask.DatabaseConnect;
using vineforceTask.DTO.Payment;
using vineforceTask.Models;
using vineforceTask.Repo.Interface;

namespace vineforceTask.Repo.Implementation
{
    public class PaymentImp : IPayment
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentImp> _logger;
        private readonly HttpClient _httpClient;

        public PaymentImp(
            ApplicationDbContext context,
            IConfiguration configuration,
            ILogger<PaymentImp> logger,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        // ============================================================
        // CREATE PAYMENT
        // ============================================================
        public async Task<Payment?> CreateAsync(CreatePaymentDto dto)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.Payment)
                    .FirstOrDefaultAsync(o => o.Id == dto.OrderId);

                if (order == null)
                {
                    _logger.LogWarning(
                        "Payment creation failed. Order {OrderId} not found.",
                        dto.OrderId
                    );

                    return null;
                }

                // Prevent duplicate payment creation
                if (order.Payment != null)
                {
                    _logger.LogWarning(
                        "Payment already exists for Order {OrderId}.",
                        dto.OrderId
                    );

                    return order.Payment;
                }

                // ====================================================
                // Razorpay Configuration
                // ====================================================
                var keyId = _configuration["Razorpay:Key"];
                var keySecret = _configuration["Razorpay:Secret"];

                if (string.IsNullOrWhiteSpace(keyId) ||
                    string.IsNullOrWhiteSpace(keySecret))
                {
                    _logger.LogError(
                        "Razorpay Key or Secret is not configured."
                    );

                    return null;
                }

                // ====================================================
                // Amount in paise
                // ====================================================
                var amountInPaise = Convert.ToInt64(
                    Math.Round(order.TotalAmount * 100)
                );

                if (amountInPaise <= 0)
                {
                    _logger.LogWarning(
                        "Invalid payment amount for Order {OrderId}: {Amount}",
                        order.Id,
                        order.TotalAmount
                    );

                    return null;
                }

                // ====================================================
                // Currency
                // ====================================================
                var currency = string.IsNullOrWhiteSpace(dto.Currency)
                    ? "INR"
                    : dto.Currency.ToUpperInvariant();

                // ====================================================
                // Razorpay Order Request
                // ====================================================
                var razorpayRequest = new
                {
                    amount = amountInPaise,
                    currency = currency,
                    receipt = $"receipt_{order.Id}",
                    notes = new
                    {
                        orderId = order.Id.ToString(),
                        customerName = order.CustomerName,
                        customerEmail = order.CustomerEmail
                    }
                };

                var json = JsonSerializer.Serialize(razorpayRequest);

                using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://api.razorpay.com/v1/orders"
                );

                request.Content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json"
                );

                // ====================================================
                // Razorpay Basic Authentication
                // ====================================================
                var authToken = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(
                        $"{keyId}:{keySecret}"
                    )
                );

                request.Headers.Authorization =
                    new AuthenticationHeaderValue(
                        "Basic",
                        authToken
                    );

                // ====================================================
                // Call Razorpay API
                // ====================================================
                var response = await _httpClient.SendAsync(request);

                var responseBody =
                    await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError(
                        "Razorpay order creation failed. Status: {StatusCode}, Response: {Response}",
                        response.StatusCode,
                        responseBody
                    );

                    return null;
                }

                // ====================================================
                // Parse Razorpay Response
                // ====================================================
                using var razorpayDocument =
                    JsonDocument.Parse(responseBody);

                var razorpayRoot =
                    razorpayDocument.RootElement;

                var razorpayOrderId =
                    razorpayRoot
                        .GetProperty("id")
                        .GetString();

                if (string.IsNullOrWhiteSpace(razorpayOrderId))
                {
                    _logger.LogError(
                        "Razorpay response did not contain a valid order ID. Response: {Response}",
                        responseBody
                    );

                    return null;
                }

                // ====================================================
                // Save Payment
                // ====================================================
                var payment = new Payment
                {
                    OrderId = order.Id,

                    RazorpayOrderId = razorpayOrderId,

                    RazorpayPaymentId = string.Empty,

                    RazorpaySignature = string.Empty,

                    Amount = order.TotalAmount,

                    Currency = currency,

                    Status = PaymentStatus.Created,

                    PaymentMethod = "Razorpay",

                    RawResponse = responseBody,

                    CreatedAt = DateTime.UtcNow
                };

                await _context.Payments.AddAsync(payment);

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Razorpay payment created successfully. " +
                    "OrderId: {OrderId}, RazorpayOrderId: {RazorpayOrderId}, Amount: {Amount}",
                    order.Id,
                    razorpayOrderId,
                    order.TotalAmount
                );

                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error creating payment for Order {OrderId}.",
                    dto.OrderId
                );

                return null;
            }
        }

        // ============================================================
        // GET PAYMENT BY ID
        // ============================================================
        public async Task<Payment?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Payments
                    .Include(p => p.Order)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting payment {PaymentId}.",
                    id
                );

                return null;
            }
        }

        // ============================================================
        // GET PAYMENT BY ORDER ID
        // ============================================================
        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            try
            {
                return await _context.Payments
                    .Include(p => p.Order)
                    .FirstOrDefaultAsync(
                        p => p.OrderId == orderId
                    );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error getting payment for Order {OrderId}.",
                    orderId
                );

                return null;
            }
        }

        // ============================================================
        // VERIFY RAZORPAY PAYMENT
        // ============================================================
        public async Task<Payment?> VerifyAsync(
            VerifyPaymentDto dto)
        {
            try
            {
                var payment = await _context.Payments
                    .FirstOrDefaultAsync(
                        p => p.OrderId == dto.OrderId
                    );

                if (payment == null)
                {
                    _logger.LogWarning(
                        "Payment verification failed. " +
                        "Payment for Order {OrderId} not found.",
                        dto.OrderId
                    );

                    return null;
                }

                // ====================================================
                // Verify Razorpay Order ID
                // ====================================================
                if (!string.Equals(
                        payment.RazorpayOrderId,
                        dto.RazorpayOrderId,
                        StringComparison.Ordinal))
                {
                    _logger.LogWarning(
                        "Razorpay Order ID mismatch for Order {OrderId}.",
                        dto.OrderId
                    );

                    payment.Status = PaymentStatus.Failed;
                    payment.FailureReason =
                        "Razorpay Order ID mismatch.";
                    payment.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    return payment;
                }

                // ====================================================
                // Verify Razorpay Signature
                // ====================================================
                var isValid = IsSignatureValid(
                    dto.RazorpayOrderId,
                    dto.RazorpayPaymentId,
                    dto.RazorpaySignature
                );

                if (!isValid)
                {
                    payment.Status = PaymentStatus.Failed;

                    payment.FailureReason =
                        "Invalid Razorpay signature.";

                    payment.UpdatedAt =
                        DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    _logger.LogWarning(
                        "Invalid Razorpay signature for Order {OrderId}.",
                        dto.OrderId
                    );

                    return payment;
                }

                // ====================================================
                // Payment Successful
                // ====================================================
                payment.RazorpayPaymentId =
                    dto.RazorpayPaymentId;

                payment.RazorpaySignature =
                    dto.RazorpaySignature;

                payment.PaymentMethod =
                    string.IsNullOrWhiteSpace(dto.PaymentMethod)
                        ? "Razorpay"
                        : dto.PaymentMethod;

                // Your enum uses Captured, not Paid
                payment.Status =
                    PaymentStatus.Captured;

                payment.FailureReason = null;

                payment.UpdatedAt =
                    DateTime.UtcNow;

                // ====================================================
                // Update Order
                // ====================================================
                var order = await _context.Orders
                    .FirstOrDefaultAsync(
                        o => o.Id == dto.OrderId
                    );

                if (order != null)
                {
                    // IMPORTANT:
                    // Use the correct value from your OrderStatus enum.
                    //
                    // If your OrderStatus enum contains Paid:
                    // order.Status = OrderStatus.Paid;
                    //
                    // If it uses another successful status,
                    // replace the line below accordingly.

                    order.Status = OrderStatus.Paid;
                    order.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Payment verified successfully for Order {OrderId}. " +
                    "RazorpayPaymentId: {PaymentId}",
                    dto.OrderId,
                    dto.RazorpayPaymentId
                );

                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error verifying payment for Order {OrderId}.",
                    dto.OrderId
                );

                return null;
            }
        }

        // ============================================================
        // UPDATE PAYMENT STATUS
        // ============================================================
        public async Task<Payment?> UpdateStatusAsync(
            int id,
            UpdatePaymentStatusDto dto)
        {
            try
            {
                var payment = await _context.Payments
                    .FirstOrDefaultAsync(
                        p => p.Id == id
                    );

                if (payment == null)
                {
                    _logger.LogWarning(
                        "Payment {PaymentId} not found.",
                        id
                    );

                    return null;
                }

                payment.Status = dto.Status;
                payment.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Payment {PaymentId} status updated to {Status}.",
                    id,
                    dto.Status
                );

                return payment;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error updating payment {PaymentId} status.",
                    id
                );

                return null;
            }
        }

        // ============================================================
        // RAZORPAY SIGNATURE VALIDATION
        // ============================================================
        private bool IsSignatureValid(
            string razorpayOrderId,
            string razorpayPaymentId,
            string signature)
        {
            var keySecret =
                _configuration["Razorpay:Secret"];

            if (string.IsNullOrWhiteSpace(keySecret))
            {
                _logger.LogError(
                    "Razorpay Secret is not configured."
                );

                return false;
            }

            if (string.IsNullOrWhiteSpace(razorpayOrderId) ||
                string.IsNullOrWhiteSpace(razorpayPaymentId) ||
                string.IsNullOrWhiteSpace(signature))
            {
                return false;
            }

            // Razorpay signature payload:
            // razorpay_order_id|razorpay_payment_id
            var payload =
                $"{razorpayOrderId}|{razorpayPaymentId}";

            using var hmac =
                new HMACSHA256(
                    Encoding.UTF8.GetBytes(keySecret)
                );

            var hash =
                hmac.ComputeHash(
                    Encoding.UTF8.GetBytes(payload)
                );

            var computedSignature =
                Convert.ToHexString(hash)
                    .ToLowerInvariant();

            return computedSignature.Equals(
                signature,
                StringComparison.OrdinalIgnoreCase
            );
        }
    }
}