using System.ComponentModel.DataAnnotations;

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
        public int duration { get; set; } // czas w minutach

        [Required]
        public decimal price { get; set; }
    }
}

