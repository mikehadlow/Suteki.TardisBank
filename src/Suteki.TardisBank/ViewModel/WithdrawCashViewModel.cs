using System.ComponentModel.DataAnnotations;

namespace Suteki.TardisBank.ViewModel
{
    public class WithdrawCashViewModel
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}