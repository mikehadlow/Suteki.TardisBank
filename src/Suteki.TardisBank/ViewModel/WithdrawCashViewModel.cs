using System.ComponentModel.DataAnnotations;

namespace Suteki.TardisBank.ViewModel
{
    public class WithdrawCashViewModel
    {
        [Required]
        [StringLength(140, ErrorMessage = "Sorry maximum of 140 chars, just like Twitter :)")]
        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}