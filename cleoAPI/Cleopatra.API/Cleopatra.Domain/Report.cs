using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleopatra.Domain
{
    public class Report
    {
        [Key]
        public int report_id { get; set; }
        public string type { get; set; }
        public DateTime created_at { get; set; }
    }

}
