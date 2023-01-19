using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.DTOS
{
    public class UserRole
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string RoleName { get; set; }
    }
}
