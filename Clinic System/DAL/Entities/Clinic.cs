using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.DAL.Entities
{
    public class Clinic
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }
        public int CityId { get; set; }
        [ForeignKey("CityId")]
        public City City { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Address { get; set; }
        public DateTime Creation { get; set; }
        public bool IsActive { get; set; }
        public string detectPrice { get; set; }
        public string RedetectPrice { get; set; }
        public int? SpecialityId { get; set; }
        [ForeignKey("SpecialityId")]
        public Speciality Speciality { get; set; }

    }
}
