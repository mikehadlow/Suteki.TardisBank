using System.ComponentModel.DataAnnotations;

namespace Suteki.TardisBank.ViewModel
{
    public class WithdrawCashForChildViewModel
    {
        public string ChildId { get; set; }
        public string ChildName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Amount { get; set; }
    }
}