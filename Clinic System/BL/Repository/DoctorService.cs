using AutoMapper;
using Clinic_System.BL.IRepsitory;
using Clinic_System.DAL.Data;
using Clinic_System.DAL.Entities;
using Clinic_System.DTOS;
using Clinic_System.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.BL.Repository
{
    public class DoctorService : IDoctorService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;

        public DoctorService(ApplicationDbContext db , IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        #region add doctor 
        public async Task<Response<string>> AddDoctorAsync(DoctorDTO model)
        {
           try
            {
                // check if doctor with user name is exist
                var doc = db.Doctors.FirstOrDefault(f => f.UserName == model.UserName);
                if(doc is not null)
                    return new Response<string> { message = "user name of doctor is already exist " };

                var newdoc = mapper.Map<Doctor>(model);
                await db.Doctors.AddAsync(newdoc);
                if(await db.SaveChangesAsync() > 0 )
                    return new Response<string> { Data = "Doctor Added Succefully " };
                return new Response<string> { message = "Doctor Not Added Succefully " };

            }
            catch
            {
                return new Response<string> { message = "doctor not added to data base " };
            }
        }
        #endregion

        #region add Clinic
        public async Task<Response<string>> AddClinicAsync(ClinicDTO model)
        {
           
            try
            {
                var doctor = await db.Doctors.FirstOrDefaultAsync(f => f.UserName == model.DoctorUserName);
                if (doctor is null)
                    return new Response<string> { message = "there is no doctor with this userName " };
                var clinic = mapper.Map<Clinic>(model);
                clinic.DoctorId = doctor.Id;
                clinic.Creation = DateTime.Now;
                clinic.IsActive = false;
                await db.Clinics.AddAsync(clinic);
                if (await db.SaveChangesAsync() > 0)
                {

                    db.ClinicManagers.Add(new ClinicManagers { ClinicId = clinic.Id, UserId = doctor.UserName });
                    db.SaveChanges();
                    return new Response<string> { Data = "clinic added succesfully" };
                }
                return new Response<string> { message = "clinic not added " };


            }
            catch
            {
                return new Response<string> { message = "there was error Ocuur" };
            }


        }

        #endregion
        #region get clinicss
        public async Task<Response<ICollection<ClinicDTO>>> GetClinicsAsync(int SprcialityId, int CityId, int Take, int Page)
        {
            try
            {
                var returndata = new List<ClinicDTO>();
                var data = await db.Clinics
                    .Where(w=>w.IsActive==true)
                    .OrderByDescending(o => o.Name)
                    .Include(i => i.Speciality)
                    .Include(i => i.Doctor)
                    .Include(i => i.City)
                    .ToListAsync();
                if (SprcialityId > 0)
                    data =  data.Where(w => w.SpecialityId == SprcialityId).ToList();
                if (CityId > 0)
                    data = data.Where(w => w.CityId == CityId).ToList();


                if (Page > 0 && Take>0)
                {
                    foreach (var item in data.Skip(Take * (Page - 1)).Take(Take))
                    {
                        var cli = mapper.Map<ClinicDTO>(item);
                        cli.CityName = item.City.Name;
                        cli.SpecialityName = item.Speciality.Name;
                        cli.DoctorName = item.Doctor.Name;
                        returndata.Add(cli);
                    }
                    int howmanypages = data.Count / Take;
                    if (data.Count % Take > 0)
                        howmanypages += 1;
                    return new Response<ICollection<ClinicDTO>> { Data = returndata, count = data.Count ,pages= howmanypages };
                }
                else
                {

                    foreach (var item in data)
                    {
                        var cli = mapper.Map<ClinicDTO>(item);
                        cli.CityName = item.City.Name;
                        cli.SpecialityName = item.Speciality.Name;
                        cli.DoctorName = item.Doctor.Name;
                        returndata.Add(cli);
                    }
                  
                    return new Response<ICollection<ClinicDTO>> { Data = returndata, count = returndata.Count};
                }
            }catch(Exception msg)
            {
                return new Response<ICollection<ClinicDTO>> { message = msg.ToString() };
            }
        }

        #endregion

        #region search clinc
        public async Task<Response<ICollection<ClinicDTO>>> SearchClinicsAsync(string Search, int Take, int Page)
        {
            try
            {
                var returndata = new List<ClinicDTO>();

                var data = await db.Clinics
                    .Include(i => i.Speciality)
                    .Include(i => i.Doctor)
                    .Include(i => i.City)
                    .Where(w => w.IsActive == true &&(w.Name.Contains(Search)
                    ||w.Speciality.Name.Contains(Search)
                    ||w.Doctor.Name.Contains(Search)
                    || w.Address.Contains(Search)
                    || w.City.Name.Contains(Search)
                    ))
                    .OrderByDescending(o => o.Name)
                    .ToListAsync();
                if (Page > 0 && Take > 0)
                {
                    foreach (var item in data.Skip(Take * (Page - 1)).Take(Take))
                    {
                        var cli = mapper.Map<ClinicDTO>(item);
                        cli.CityName = item.City.Name;
                        cli.SpecialityName = item.Speciality.Name;
                        cli.DoctorName = item.Doctor.Name;
                        returndata.Add(cli);
                    }
                    int howmanypages = data.Count / Take;
                    if (data.Count % Take > 0)
                        howmanypages += 1;
                    return new Response<ICollection<ClinicDTO>> { Data = returndata, count = data.Count, pages = howmanypages };
                }
                else
                {

                    foreach (var item in data)
                    {
                        var cli = mapper.Map<ClinicDTO>(item);
                        cli.CityName = item.City.Name;
                        cli.SpecialityName = item.Speciality.Name;
                        cli.DoctorName = item.Doctor.Name;
                        returndata.Add(cli);
                    }

                    return new Response<ICollection<ClinicDTO>> { Data = returndata, count = returndata.Count };
                }
            }
            catch(Exception ex)
            {
                return new Response<ICollection<ClinicDTO>> { Data=null ,message = ex.ToString() };
            }
        }

        #endregion
    }
}
