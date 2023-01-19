using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.DTOS
{
    public class ClinicDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //public int DoctorId { get; set; }
        public string DoctorUserName { get; set; }
  
        public string DoctorName { get; set; }
        public int CityId { get; set; }
   
        public string CityName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Address { get; set; }
        public DateTime Creation { get; set; }
        public bool IsActive { get; set; }
        public string detectPrice { get; set; }
        public string RedetectPrice { get; set; }
        public int? SpecialityId { get; set; }
     
        public string SpecialityName { get; set; }
    }
}
