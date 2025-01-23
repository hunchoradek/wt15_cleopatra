using Salon.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cleopatra.Domain
{
    public class Vacation
    {
        [Key]
        public int vacation_id { get; set; }

        [Required]
        public int employee_id { get; set; }

        [ForeignKey("employee_id")]
        public Employee Employee { get; set; }

        [Required]
        public DateTime start_date { get; set; }

        [Required]
        public DateTime end_date { get; set; }
    }
}
