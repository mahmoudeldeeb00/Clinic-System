using Clinic_System.DTOS;
using Clinic_System.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.BL.IRepsitory
{
    public interface IDoctorService
    {
        Task<Response<string>> AddDoctorAsync(DoctorDTO model);
        Task<Response<string>> AddClinicAsync(ClinicDTO model);
        Task<Response<ICollection<ClinicDTO>>> GetClinicsAsync(int SprcialityId, int CityId, int Take, int Page);
        Task<Response<ICollection<ClinicDTO>>> SearchClinicsAsync(string Search, int Take, int Page);
    }
}
