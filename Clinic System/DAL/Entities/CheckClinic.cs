using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.DAL.Entities
{
    public class CheckClinic
    {
        [Key]
        public int Id { get; set; }
        public int number { get; set; }
        public DateTime Day { get; set; }
        public string UserName { get; set; }
        public int ClinicId { get; set; }
        [ForeignKey("ClinicId")]
        public Clinic Clinic { get; set; }
        public bool IsReCheck { get; set; }
        public bool IsFinished { get; set; }
    }
}
