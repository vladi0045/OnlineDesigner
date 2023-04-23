using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineDesigner.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Design")]
        public int DesignId { get; set; }

        public Design Design { get; set; }

        public int Quantity { get; set; }
    }
}
