using System.ComponentModel.DataAnnotations;

namespace OnlineDesigner.Models
{
    public class Type
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Item> Item { get; set; }
    }
}
