using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.DAL.Entities
{
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        public string  UserName { get; set; }
        public string  PhoneNumber { get; set; }
        public string Name { get; set; }
        public string PhotoSrc { get; set; }
        public DateTime BirthDate { get; set; }

        public int SpecialityId { get; set; }
        [ForeignKey("SpecialityId")]
        public Speciality Speciality { get; set; }


        public ICollection<Clinic> Clinics { get; set; }
    }
}
