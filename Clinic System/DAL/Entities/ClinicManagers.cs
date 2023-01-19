using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.DAL.Entities
{
    public class ClinicManagers
    {
        [Key]
        public int Id { get; set; }
        public string UserId  { get; set; }
        public int ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; }
    }
}
