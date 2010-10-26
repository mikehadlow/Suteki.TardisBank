using System.ComponentModel.DataAnnotations;

namespace Suteki.TardisBank.ViewModel
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "You must supply an email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "You must supply a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must supply a password")]
        public string Password { get; set; }
    }
}