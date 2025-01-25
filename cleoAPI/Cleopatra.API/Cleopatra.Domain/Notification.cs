using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Cleopatra.Domain
{
    public class Notification
    {
        [Key]
        public int notification_id { get; set; }

        [Required]
        public int client_id { get; set; }

        [ForeignKey("client_id")]
        [JsonIgnore] // Ignoruj nawigację w serializacji
        public Client? Client { get; set; }

        [Required]
        [MaxLength(10)]
        public string type { get; set; }  // email or sms

        [Required]
        public string content { get; set; }

        public DateTime sent_at { get; set; } = DateTime.Now;


    }
}
