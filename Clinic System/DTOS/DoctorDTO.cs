using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.DTOS
{
    public class DoctorDTO
    {
  
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        [Required]

        public string Name { get; set; }
        public string PhotoSrc { get; set; }
        public DateTime BirthDate { get; set; }

        public int SpecialityId { get; set; }
   
        public string SpecialityName { get; set; }


        public ICollection<ClinicDTO> Clinics { get; set; }
    }
}
