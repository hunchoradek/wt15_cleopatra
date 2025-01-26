using Salon.Domain;
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
        public Client Client { get; set; }

        [Required]
        public int employee_id { get; set; } // Dodaj to pole

        [ForeignKey("employee_id")]
        public Employee Employee { get; set; } // Powiązanie z pracownikiem

        [Required]
        public string employee_name { get; set; }

        [Required]
        public string service { get; set; }

        [Required]
        public DateTime appointment_date { get; set; } // Dodaj to pole

        public bool ReminderSent { get; set; } // New property

        [Required]
        public TimeSpan start_time { get; set; }

        [Required]
        public TimeSpan end_time { get; set; }

        public string status { get; set; }
        public string? notes { get; set; }
    }
}
