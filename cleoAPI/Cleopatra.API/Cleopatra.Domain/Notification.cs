using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cleopatra.Domain
{
    public class Notification
    {
        [Key]
        public int notification_id { get; set; }

        [Required]
        public int client_id { get; set; }

        // Navigation property
        [ForeignKey("client_id")]
        public Client client { get; set; }

        [Required]
        [MaxLength(10)]
        public string type { get; set; }  // email or sms

        [Required]
        public string content { get; set; }

        public DateTime sent_at { get; set; } = DateTime.Now;


    }
}
