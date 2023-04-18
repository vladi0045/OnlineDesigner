using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineDesigner.Models
{
    public class Design
    {
        [Key]
        public int Id { get; private set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }

        public Item Item { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        public string Color { get; set; }

        public string Img { get; set; }
    }
}
