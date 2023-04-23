using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineDesigner.Models
{
    public class Order
    {
        [Key]
        public int Id { get; private set; }

        public IEnumerable<CartItem> CartItems { get; set; }

        public bool Status { get; set; }

        public double Price { get; set; }

        public string PaymentMethod { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
