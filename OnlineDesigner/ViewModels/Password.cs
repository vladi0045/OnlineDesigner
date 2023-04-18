using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineDesigner.ViewModels
{
    public class Password
    {
        [Required, DisplayName("Current password")]
        public string CurrentPassword { get; set; }
        [Required, DisplayName("New password")]
        public string NewPassword { get; set; }
        [Required, DisplayName("Confirm password"), Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
