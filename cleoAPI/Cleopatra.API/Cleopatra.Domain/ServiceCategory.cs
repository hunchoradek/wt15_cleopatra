using System.ComponentModel.DataAnnotations;

namespace Cleopatra.Domain
{
    public class ServiceCategory
    {
        [Key]
        public int category_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }
    }
}
