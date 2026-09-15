using Microsoft.AspNetCore.Mvc;
using vineforceTask.DTO;
using vineforceTask.DTO.Payment;
using vineforceTask.Repo.Interface;

namespace vineforceTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentCrud : ControllerBase
    {
        private readonly IPayment _paymentRepository;

        public PaymentCrud(IPayment paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        // GET: api/PaymentCrud/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);

            if (payment == null)
            {
                return NotFound(new
                {
                    message = "Payment not found."
                });
            }

            return Ok(payment);
        }

        // GET: api/PaymentCrud/order/1
        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrder(int orderId)
        {
            var payment = await _paymentRepository.GetByOrderIdAsync(orderId);

            if (payment == null)
            {
                return NotFound(new
                {
                    message = "No payment found for this order."
                });
            }

            return Ok(payment);
        }

        // POST: api/PaymentCrud
        // Starts a payment for an existing order (creates the Razorpay order server-side).
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreatePaymentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = await _paymentRepository.CreateAsync(dto);

            if (payment == null)
            {
                return BadRequest(new
                {
                    message = "Could not create payment. Check that the order exists and doesn't already have one."
                });
            }

            return CreatedAtAction(
                nameof(GetById),
                new { id = payment.Id },
                payment);
        }

        // POST: api/PaymentCrud/verify
        // Called after Razorpay's checkout completes on the client, to verify
        // the signature server-side and mark the order/payment as paid.
        [HttpPost("verify")]
        public async Task<IActionResult> Verify(
            [FromBody] VerifyPaymentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = await _paymentRepository.VerifyAsync(dto);

            if (payment == null)
            {
                return NotFound(new
                {
                    message = "No payment found for this order."
                });
            }

            if (payment.Status != Models.PaymentStatus.Captured)
            {
                return BadRequest(new
                {
                    message = "Payment verification failed.",
                    payment
                });
            }

            return Ok(payment);
        }

        // PUT: api/PaymentCrud/1/status
        // Manual status update - e.g. marking a payment Failed or Refunded.
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdatePaymentStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var payment = await _paymentRepository.UpdateStatusAsync(id, dto);

            if (payment == null)
            {
                return NotFound(new
                {
                    message = "Payment not found."
                });
            }

            return Ok(payment);
        }
    }
}