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
        #region Helper Methods
        private ICollection<ClinicDTO> GetClinicsToDoctor(int Id)=>mapper.Map<List<ClinicDTO>>( db.Clinics.Where(w => w.DoctorId == Id).ToList());
        public static int ExtractNumberFromString(string word)
        {
            try
            {

                string newOne = "";
                char[] characters = word.ToCharArray();
                foreach (var c in characters)
                {
                    try
                    {
                        int number = Int32.Parse(c.ToString());
                        newOne = newOne + number;
                    }
                    catch
                    {

                    }
                }
                return Int32.Parse(newOne);
            }
            catch
            {
                return 0;

            }
        }

        #endregion

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

        #region get clinics
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

        #region getclinic by id
        public async Task<Response<ClinicDTO>> GetClininByIdAsync(int Id)
        {
            var clinic = await db.Clinics
                .Include(i => i.Doctor)
                .Include(i => i.Speciality)
                .Include(i => i.City)
                .FirstOrDefaultAsync(f => f.Id == Id);
            if (clinic is null)
                return new Response<ClinicDTO> {Data=null ,  message = "there is no clinics with id " };
            var ClinicDto = mapper.Map<ClinicDTO>(clinic);
            ClinicDto.SpecialityName = clinic.Speciality.Name;
            ClinicDto.DoctorName = clinic.Doctor.Name;
            ClinicDto.DoctorUserName = clinic.Doctor.UserName;
            ClinicDto.CityName = clinic.City.Name;
            return new Response<ClinicDTO> { Data = ClinicDto };
        }
        #endregion

        #region get dectors

        public async Task<Response<ICollection<DoctorDTO>>> GetDoctorsAsync(int SprcialityId, int Take, int Page)
        {
            try
            {
                var returndata = new List<DoctorDTO>();
                var data = await db.Doctors
                    .OrderByDescending(o => o.Name)
                    .Include(i => i.Speciality)
                    .ToListAsync();
                if (SprcialityId > 0)
                    data = data.Where(w => w.SpecialityId == SprcialityId).ToList();
              

                if (Page > 0 && Take > 0)
                {
                    foreach (var item in data.Skip(Take * (Page - 1)).Take(Take))
                    {
                        var cli = mapper.Map<DoctorDTO>(item);
                        cli.SpecialityName = item.Speciality.Name;
                        cli.Clinics = GetClinicsToDoctor(item.Id);
                        returndata.Add(cli);
                    }
                    int howmanypages = data.Count / Take;
                    if (data.Count % Take > 0)
                        howmanypages += 1;
                    return new Response<ICollection<DoctorDTO>> { Data = returndata, count = data.Count, pages = howmanypages };
                }
                else
                {

                    foreach (var item in data)
                    {
                        var cli = mapper.Map<DoctorDTO>(item);
                        cli.SpecialityName = item.Speciality.Name;
                        cli.Clinics = GetClinicsToDoctor(item.Id);
                        returndata.Add(cli);
                    }

                    return new Response<ICollection<DoctorDTO>> { Data = returndata, count = returndata.Count };
                }
            }
            catch (Exception msg)
            {
                return new Response<ICollection<DoctorDTO>> { message = msg.ToString() };
            }
        }

        #endregion

        #region search doctors
        public async Task<Response<ICollection<DoctorDTO>>> SearchDoctorsAsync(string Search, int Take, int Page)
        {
            try
            {
                var returndata = new List<DoctorDTO>();

                var data = await db.Doctors
                    .Include(i => i.Speciality)                    
                    .Where(w => w.Name.Contains(Search)
                    || w.Speciality.Name.Contains(Search)
                    || w.Name.Contains(Search)
                    || w.UserName.Contains(Search)
                    )
                    .OrderByDescending(o => o.Name)
                    .ToListAsync();
                if (Page > 0 && Take > 0)
                {
                    foreach (var item in data.Skip(Take * (Page - 1)).Take(Take))
                    {
                        var cli = mapper.Map<DoctorDTO>(item);
                        cli.SpecialityName = item.Speciality.Name;
                        cli.Clinics = GetClinicsToDoctor(item.Id);
                        returndata.Add(cli);
                    }
                    int howmanypages = data.Count / Take;
                    if (data.Count % Take > 0)
                        howmanypages += 1;
                    return new Response<ICollection<DoctorDTO>> { Data = returndata, count = data.Count, pages = howmanypages };
                }
                else
                {

                    foreach (var item in data)
                    {
                        var cli = mapper.Map<DoctorDTO>(item);
                        cli.SpecialityName = item.Speciality.Name;
                        cli.Clinics =GetClinicsToDoctor(item.Id);
                        returndata.Add(cli);
                    }

                    return new Response<ICollection<DoctorDTO>> { Data = returndata, count = returndata.Count };
                }
            }
            catch (Exception ex)
            {
                return new Response<ICollection<DoctorDTO>> { Data = null, message = ex.ToString() };
            }
        }

        #endregion

        #region get doctor by id 
        public async Task<Response<DoctorDTO>> GetDoctorByIdAsync(int Id)
        {
            var doctor = await db.Doctors
                .Include(i => i.Speciality)
                .FirstOrDefaultAsync(f => f.Id == Id);
            if (doctor is null)
                return new Response<DoctorDTO> { Data = null, message = "there is no clinics with id " };
           var  DoctorDto = mapper.Map<DoctorDTO>(doctor);
            DoctorDto.SpecialityName = doctor.Speciality.Name;
            DoctorDto.Clinics = GetClinicsToDoctor(doctor.Id);
            return new Response<DoctorDTO> { Data = DoctorDto };
        }

        #endregion

        #region finish check and gains 
        public async Task<Response<CheckClinic>> CheckThisClinicAsync(int ClinicId, string UserName, int d, int m, int y , bool IsReCheck)
        {
            Clinic x = await db.Clinics.FirstOrDefaultAsync(f => f.Id == ClinicId && f.IsActive == true);
            if(x is not null)
            {
                DateTime date = new DateTime(y, m, d);
                TimeSpan s = date - DateTime.Now;
                if (s.Days < 0)
                    return new Response<CheckClinic> { message = "PLZ enter date in future not in the past " };
                if (s.Days > 15)
                    return new Response<CheckClinic> { message = "PLZ enter date in tthe next 15 days  " };


                var checkkifexist = await db.CheckClinics.FirstOrDefaultAsync(f => f.Day.Year ==y &&f.Day.Month==m &&f.Day.Day==d && f.ClinicId == ClinicId && f.UserName == UserName && f.IsFinished ==false);
                if (checkkifexist is null)
                {
                    int checknumber;
                    try  {
                        checknumber = ( db.CheckClinics.Where(f => f.Day.Year == y && f.Day.Month == m && f.Day.Day == d).Select(s=>s.number).ToArray().Max()) + 1;
                    }
                    catch
                    {
                        checknumber = 1;
                    }
                    var check = new CheckClinic
                    {
                        ClinicId = ClinicId,
                        Day = date,
                        IsFinished = false,
                        IsReCheck = IsReCheck,
                        UserName = UserName,
                        number = checknumber
                    };
                    var result = await db.CheckClinics.AddAsync(check);
                    if(await db.SaveChangesAsync() > 0)
                    {
                        return new Response<CheckClinic> { Data = check };
                    }
                    return new Response<CheckClinic> { Data = null, message = "the check doesn't save " };
                } 
                else {
                    return new Response<CheckClinic> { Data = null, message = "You are already check this clinic in this day  " };
                }


            }
            else
            {
                return new Response<CheckClinic> { Data = null, message = "there is no clinics do you want to check " };
            }


        }

        #endregion

        #region finish Check 

       public async Task<Response<string>> FinishCheckAsync(int CheckId)
        {
            var check =await  db.CheckClinics.Include(i=>i.Clinic).FirstOrDefaultAsync(f => f.Id == CheckId&& f.IsFinished == false);
            if(check is not null)
            {
                check.IsFinished = true;
               if( await db.SaveChangesAsync()>0)
                {
                    int price;
                    if(check.IsReCheck == true)
                    {
                        price = ExtractNumberFromString(check.Clinic.RedetectPrice);
                    }
                    else
                    {
                        price = ExtractNumberFromString(check.Clinic.detectPrice);

                    }
                        await db.Gains.AddAsync(new Gain { CheckClinicId = CheckId, ClinicId = check.ClinicId, Date = DateTime.Now, Money = price });
                    if(await db.SaveChangesAsync() > 0)
                    {
                        return new Response<string> { Data = "Check Finished and Gains Collected Succesfully" };
                    }
                        return new Response<string> { message = "Check Finished but  Gains Not  Collected " };

                }
                else
                {
                    return new Response<string> { message = "Check not finished Yet " };
                }
            }
            else
            {
                return new Response<string> { message = "there is no checks with this Id " };
            }
        }

        #endregion
    }
}
