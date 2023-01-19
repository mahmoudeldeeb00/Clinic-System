using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.DAL.Entities
{
    public class Gain
    {
        [Key]
        public int Id { get; set; }
        public int Money { get; set; }
        public int? ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; }
        public DateTime Date { get; set; }
        public int CheckClinicId { get; set; }
        [ForeignKey("CheckClinicId")]
        public CheckClinic CheckClinic { get; set; }

    }
}
