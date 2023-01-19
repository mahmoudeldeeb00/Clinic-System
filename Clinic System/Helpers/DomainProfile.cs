using AutoMapper;
using Clinic_System.DAL.Entities;
using Clinic_System.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.Helpers
{
    public class DomainProfile:Profile
    {
        public DomainProfile()
        {
            CreateMap<DoctorDTO, Doctor>();
            CreateMap<Doctor, DoctorDTO>();


            CreateMap<Clinic, ClinicDTO>();
            CreateMap<ClinicDTO, Clinic>();
        }

    }
}
