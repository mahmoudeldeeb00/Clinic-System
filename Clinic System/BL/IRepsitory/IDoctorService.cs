using Clinic_System.DAL.Entities;
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
        Task<Response<ClinicDTO>> GetClininByIdAsync(int Id);
        Task<Response<ICollection<DoctorDTO>>> GetDoctorsAsync(int SprcialityId, int Take, int Page);
        Task<Response<ICollection<DoctorDTO>>> SearchDoctorsAsync(string Search, int Take, int Page);
        Task<Response<DoctorDTO>> GetDoctorByIdAsync(int Id);
        Task<Response<CheckClinic>> CheckThisClinicAsync(int ClinicId,string UserName, int d, int m, int y, bool IsReCheck);
        Task<Response<string>> FinishCheckAsync(int CheckId); 
    }
}
