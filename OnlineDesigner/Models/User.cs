using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineDesigner.Models
{
    public class User : IdentityUser
    {
        [Required]
        public Role Role { get; set; }

        public IEnumerable<Order>? Orders { get; set; }
    }
}
