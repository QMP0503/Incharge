using System.ComponentModel.DataAnnotations;

namespace Incharge.ViewModels

{
    public class LoginVM
    {
        [Required(ErrorMessage = " is required")] //no need for "username" text because of login format
        public string Email { get; set; } //email will be the username
        [Required(ErrorMessage = " is required")]
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }    
    }
}
