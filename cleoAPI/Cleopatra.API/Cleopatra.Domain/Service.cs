using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cleopatra.Domain
{
    public class Service
    {
        [Key]
        public int service_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        public string description { get; set; }

        [Required]
        public int duration { get; set; } // Czas w minutach

        [Required]
        public decimal price { get; set; }

        [Required]
        public int category_id { get; set; }

        [ForeignKey("category_id")]
        public ServiceCategory category { get; set; }
    }
}
