using System;
using System.ComponentModel.DataAnnotations;
using Suteki.TardisBank.Model;

namespace Suteki.TardisBank.ViewModel
{
    public class AddScheduleViewModel
    {
        [Required]
        public string ChildId { get; set; }

        [Required]
        [Range(-1000000, 1000000)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(140, ErrorMessage = "Sorry maximum of 140 chars, just like Twitter :)")]
        public string Description { get; set; }

        [Required]
        public Interval Interval { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
    }
}