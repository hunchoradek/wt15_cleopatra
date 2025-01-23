using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salon.Domain
{
    public class Employee
    {
        [Key]
        public int employee_id { get; set; }

        [Required]
        [MaxLength(100)]
        public string name { get; set; }

        [Required]
        [MaxLength(100)]
        public string email { get; set; }

        [Required]
        public string username { get; set; }

        [MaxLength(15)]
        public string phone_number { get; set; }

        [Required]
        public string role { get; set; } // "manager" lub "worker"

        [Required]
        public string password_hash { get; set; }

        [Required]
        public string working_hours { get; set; } // JSON z godzinami pracy

        [Required]
        public string specialties { get; set; } // JSON z kategoriami usług

        public bool isDeleted { get; set; } = false; // Pseudo-usunięcie
    }
}
