using Clinic_System.DAL.Data;
using Clinic_System.DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clinic_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelperController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _rolmng;
        private readonly UserManager<AppUser> user;

        public HelperController(ApplicationDbContext db, RoleManager<IdentityRole> rolmng,UserManager<AppUser>user)
        {
            this._db = db;
            this._rolmng = rolmng;
            this.user = user;
        }

        [HttpGet("FetchCities")]
        public IActionResult FetchCities ()=> Ok(_db.Cities.Select(a => new City { Id = a.Id, Name = a.Name }).ToList());
        [HttpGet("FetchGenders")]
        public IActionResult  FetchGenders ()=> Ok(_db.Genders.Select(a => new Gender { Id = a.Id, Name = a.Name }).ToList());
        [HttpGet("FetchRoles")]
        public IActionResult FetchRoles() => Ok(_rolmng.Roles.ToList());
        [HttpGet("FetchUserRoles")]
        public IActionResult FetchUserRoles()
        {
            var UserName = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(user.GetRolesAsync(user.FindByNameAsync(UserName).Result).Result.ToList());
        }

        [HttpGet("FetchSpecialities")]
        public IActionResult FetchSpecialities() => Ok(_db.Specialities.ToList());

    }
}
