using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleopatra.Domain
{
    public class UpdatePasswordRequest
    {
        [Required]
        public string password { get; set; }
    }

}
