using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Cleopatra.Domain
{
    public class Client
    {
        [Key]
        public int client_id { get; set; }
        [Required]
        [MaxLength(100)]
        public string name { get; set; }
        [MaxLength(15)]
        public string? phone_number { get; set; }
        [MaxLength(100)]
        public string? email { get; set; }
        public string? notes { get; set; }

        public bool is_deleted { get; set; } = false;

        // Navigation property (ignored in JSON)
        [JsonIgnore]
        public ICollection<Notification>? Notifications { get; set; }

    }
}
