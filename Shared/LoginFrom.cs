using System.ComponentModel.DataAnnotations;

namespace Mediwatch.Shared
{
    public class LoginForm
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
