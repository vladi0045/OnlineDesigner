using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineDesigner.ViewModels
{
    public class Login
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, DisplayName("Remember me?")]
        public bool RememberMe { get; set; }
    }
}
