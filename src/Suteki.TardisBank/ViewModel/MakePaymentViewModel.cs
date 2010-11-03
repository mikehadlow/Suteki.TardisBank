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
        [StringLength(140, ErrorMessage = "Sorry maximum of 140 chars, just like Twitter :)")]
        public string Description { get; set; }

        [Required]
        [Range(0, 1000000)]
        public decimal Amount { get; set; }
    }
}