using System.ComponentModel.DataAnnotations;

namespace Salon.Domain
{
    public class Business
    {
        [Key]
        public int business_id { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string opening_hours { get; set; } // JSON z godzinami otwarcia
    }
}
