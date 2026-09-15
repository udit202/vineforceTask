using System.ComponentModel.DataAnnotations;
using vineforceTask.Models;

namespace vineforceTask.DTO.Payment
{
    public class UpdatePaymentStatusDto
    {
        [Required]
        public PaymentStatus Status { get; set; }

        public string? FailureReason { get; set; }
    }

}
