using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineDesigner.Models
{
    public class Item
    {

        [Key]
        public int Id { get; private set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("Type")]
        public string TypeId { get; set; }
        public Type Type { get; set; }

        [Required]
        public string Sex { get; set; }

        public string Img { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
