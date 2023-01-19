using Clinic_System.BL.IRepsitory;
using Clinic_System.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdmiService adminserv;

        public AdminController(IAdmiService adminserv)
        {
            this.adminserv = adminserv;
        }
        [HttpPost("SignUserToRole")]
        public IActionResult SignUserToRole([FromBody]UserRole model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(adminserv.SignUserToRole(model));
        }
        [HttpPost("AddSpeciality")]
        public IActionResult AddSpeciality([FromQuery] string model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(adminserv.AddSpeciality(model));
        }
        [HttpGet("GetClinicsNotActive")]
        public IActionResult getclinicsnotactive()
        {
            return Ok(adminserv.GetClinicsNotActive());
        }
        [HttpPost("ActiveClinic")]
        public IActionResult ActiveClinic([FromQuery] int Id)
        {
            return Ok(adminserv.ActiveClinic(Id));
        }
    }
}
