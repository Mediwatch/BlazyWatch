using System.ComponentModel.DataAnnotations;

namespace Mediwatch.Shared
{
    public class RegisterForm
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string UserName  { get; set; }
        
        [Required]
        [EmailAddress]
        public string EmailAddress  { get; set; }
        
        [Required]
        public string Password  { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string PasswordConfirm { get; set; }
    }
}