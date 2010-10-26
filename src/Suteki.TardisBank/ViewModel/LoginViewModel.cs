using System.ComponentModel.DataAnnotations;

namespace Suteki.TardisBank.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "You must enter your user name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "You must enter your password")]
        public string Password { get; set; }
    }
}