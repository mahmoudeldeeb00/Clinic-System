using Clinic_System.DTOS;
using Clinic_System.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.BL.IRepsitory
{
   public interface IAdmiService
    {

        Response<string> SignUserToRole(UserRole model);
        Response<string> AddSpeciality(string name);
        Response<ICollection<ClinicDTO>> GetClinicsNotActive();
        Response<string> ActiveClinic(int ClinicId);

    }
}
