using System.ComponentModel.DataAnnotations;

namespace Suteki.TardisBank.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "You must enter your user name")]
        [StringLength(140, ErrorMessage = "Sorry maximum of 140 chars, just like Twitter :)")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "You must enter your password")]
        [StringLength(140, ErrorMessage = "Sorry maximum of 140 chars, just like Twitter :)")]
        public string Password { get; set; }
    }
}