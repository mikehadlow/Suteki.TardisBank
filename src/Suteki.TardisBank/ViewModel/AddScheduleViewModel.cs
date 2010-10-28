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
        public decimal Amount { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public Interval Interval { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
    }
}