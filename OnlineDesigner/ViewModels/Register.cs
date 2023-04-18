using OnlineDesigner.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineDesigner.ViewModels
{
    public class Register : IValidatableObject
    {
        public string? Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required, DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public Role Role { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password.Length < 8)
            {
                yield return new ValidationResult("Password must be at least 8 symbols long!",
                    new[] { nameof(Password) });
            }
            if (Password != ConfirmPassword)
            {
                yield return new ValidationResult("Passwords must match!",
                    new[] { nameof(Password), nameof(ConfirmPassword) });
            }
        }
    }
}
