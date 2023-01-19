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
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService docServ;

        public DoctorController(IDoctorService DocServ)
        {
            docServ = DocServ;
        }

       [HttpPost("AddDoctor")]
       public async Task<IActionResult>AddDoctor([FromBody]DoctorDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await docServ.AddDoctorAsync(model));
        }
        [HttpPost("AddClinic")]
        public async Task<IActionResult> AddClinic([FromBody]ClinicDTO model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await docServ.AddClinicAsync(model));
        }
        [HttpGet("GetClinics")]
        public async Task<IActionResult> GetClinics([FromQuery]int SprcialityId, [FromQuery] int CityId , [FromQuery] int Take =0 , [FromQuery] int Page=0)
        {
            return Ok(await docServ.GetClinicsAsync(SprcialityId, CityId, Take, Page));
        }
        [HttpGet("SearchClinics")]
        public async Task<IActionResult> SearchClinics( [FromQuery] string Search, [FromQuery] int Take = 0, [FromQuery] int Page = 0)
        {
            return Ok(await docServ.SearchClinicsAsync(Search, Take, Page));
        }
    }
}
