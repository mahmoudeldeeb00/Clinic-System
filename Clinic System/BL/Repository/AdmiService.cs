using AutoMapper;
using Clinic_System.BL.IRepsitory;
using Clinic_System.DAL.Data;
using Clinic_System.DAL.Entities;
using Clinic_System.DTOS;
using Clinic_System.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.BL.Repository
{
    public class AdmiService:IAdmiService
    {
        private readonly UserManager<AppUser> _user;
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public AdmiService(UserManager<AppUser> user,ApplicationDbContext db,IMapper mapper)
        {
            _user = user;
            this.db = db;
            this.mapper = mapper;
        }
        
        public Response<string> SignUserToRole(UserRole model)
        {
           
            try
            {
                var user =  _user.FindByNameAsync(model.UserName).Result;
                if (user is null)
                    return new Response<string> { message = "there is no user with this user name" };
                if ( _user.IsInRoleAsync(user, model.RoleName).Result)
                    return new Response<string> { message = " user is already in role " };
                var result =  _user.AddToRoleAsync(user, model.RoleName).Result;
                return result.Succeeded ? new Response<string> { Data = " user added succefully " }: new Response<string> { message = " user not added  to role " };

            }
            catch
            {
                return new Response<string> { message = " user not added  to role " };

            }

        }
        public  Response<string> AddSpeciality(string name)
        {
            var x = db.Specialities.FirstOrDefault(f => f.Name == name);
            if(x is not null)
                return new Response<string> { message = "هذا التخصص موجود بالفعل"};
            var newspeciality = new Speciality { Name = name };
            var result =  db.Specialities.AddAsync(newspeciality).Result;
            return  db.SaveChangesAsync().Result > 0 ? new Response<string> { Data = "speciality added Succefully " } : new Response<string> { message = "speciality no added yet !" };
        }

        public Response<ICollection<ClinicDTO>> GetClinicsNotActive()
        {
            var collection = new List<ClinicDTO>();
            foreach(var item in db.Clinics.Include(i => i.Doctor).Include(i=>i.City).Include(i=>i.Speciality).Where(w => w.IsActive == false).OrderBy(o => o.Name))
            {
                ClinicDTO one = mapper.Map<ClinicDTO>(item);
                one.SpecialityName = item.Speciality.Name;
                one.CityName = item.City.Name;
                one.DoctorName = item.Doctor.Name;
                collection.Add(one);
            }
            if (collection.IsNullOrEmpty())
                return new Response<ICollection<ClinicDTO>> { message = "there is No Clinics " };
           
            return new Response<ICollection<ClinicDTO>> { Data = collection ,count=collection.Count };
        }
        public Response<string> ActiveClinic(int ClinicId)
        {
            var clinic = db.Clinics.FirstOrDefault(w => w.Id == ClinicId);
            clinic.IsActive = true;
            if (db.SaveChanges() > 0)
                return new Response<string> { Data = $"Clinic {clinic.Name} is Now Active" };
            return new Response<string> { message = "Clinic is still Not Active " };


        }


    }
}
