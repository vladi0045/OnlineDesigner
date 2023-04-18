using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineDesigner.Models
{
    public class Order
    {
        [Key]
        public int Id { get; private set; }

        public IEnumerable<Design> Designs { get; set; }

        public bool Status { get; set; }

        public int Price { get; set; }

        public string PaymentMethod { get; set; }
    }
}
