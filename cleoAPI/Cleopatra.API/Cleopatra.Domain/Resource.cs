using System.ComponentModel.DataAnnotations;

namespace Cleopatra.Domain
{
    public class Resource
    {
        [Key]
        public int resource_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        [Required]
        public int quantity { get; set; }

        [MaxLength(20)]
        public string unit { get; set; }

        public int reorder_level { get; set; }
    }
}