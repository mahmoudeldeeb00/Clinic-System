using Clinic_System.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.DAL.Data
{
    public class ApplicationDbContext:IdentityDbContext<AppUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> App):base(App)
        {

        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<ClinicManagers> ClinicManagers { get; set; }
        public DbSet<CheckClinic> CheckClinics { get; set; }
        public DbSet<Gain> Gains { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }

    }
}
