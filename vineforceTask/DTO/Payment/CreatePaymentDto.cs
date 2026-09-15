using System.ComponentModel.DataAnnotations;

namespace vineforceTask.DTO.Payment
{
    public class CreatePaymentDto
    {
        [Required]
        public int OrderId { get; set; }

        public string Currency { get; set; } = "INR";
    }
}
