using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cleopatra.Domain
{
    public class Appointment
    {
        [Key]
        public int appointment_id { get; set; }

        [Required]
        public int client_id { get; set; }

        [ForeignKey("client_id")]
        public Client client { get; set; }

        [Required]
        [MaxLength(100)]
        public string employee_name { get; set; }

        [Required]
        [MaxLength(100)]
        public string service { get; set; }

        [Required]
        public DateTime appointment_date { get; set; }

        [Required]
        public TimeSpan start_time { get; set; }

        [Required]
        public TimeSpan end_time { get; set; }

        public string? status { get; set; }
        public string? notes { get; set; }


    }
}

