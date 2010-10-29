using System.ComponentModel.DataAnnotations;

namespace Suteki.TardisBank.ViewModel
{
    public class ForgottenPasswordViewModel
    {
        [Required(ErrorMessage = "You must enter a User Name or Password")]
        public string UserName { get; set; }
    }
}