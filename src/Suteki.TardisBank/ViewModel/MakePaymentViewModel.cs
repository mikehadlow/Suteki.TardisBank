using System.ComponentModel.DataAnnotations;

namespace Suteki.TardisBank.ViewModel
{
    public class MakePaymentViewModel
    {
        [Required]
        public string ChildId { get; set; }

        [Required]
        public string ChildName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}