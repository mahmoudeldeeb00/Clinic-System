using Clinic_System.BL.IRepsitory;
using Clinic_System.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService docServ;

        public DoctorController(IDoctorService DocServ)
        {
            docServ = DocServ;
        }

        #region Doctor Region ----------------------------
        #region Add
        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddDoctor([FromBody] DoctorDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await docServ.AddDoctorAsync(model));
        }
        #endregion

        #region Get 

        [HttpGet("GetDoctors")]
        public async Task<IActionResult> GetDoctors([FromQuery] int SprcialityId, [FromQuery] int Take = 0, [FromQuery] int Page = 0)
        {
            return Ok(await docServ.GetDoctorsAsync(SprcialityId, Take, Page));
        }
        #endregion

        #region search

        [HttpGet("SearchDoctors")]
        public async Task<IActionResult> SearchDoctors([FromQuery] string Search, [FromQuery] int Take = 0, [FromQuery] int Page = 0)
        {
            return Ok(await docServ.SearchClinicsAsync(Search, Take, Page));
        }
        #endregion

        #region get ByID 
        [HttpGet("GetDoctor")]
        public async Task<IActionResult> GetDoctor([FromQuery] int Id) => Ok(await docServ.GetDoctorByIdAsync(Id));
        #endregion
        #endregion


        #region Clinics Region -------------------------

        #region Add
        [HttpPost("AddClinic")]
        public async Task<IActionResult> AddClinic([FromBody] ClinicDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(await docServ.AddClinicAsync(model));
        }


        #endregion

        #region Get 
        [HttpGet("GetClinics")]
        public async Task<IActionResult> GetClinics([FromQuery] int SprcialityId, [FromQuery] int CityId, [FromQuery] int Take = 0, [FromQuery] int Page = 0)
        {
            return Ok(await docServ.GetClinicsAsync(SprcialityId, CityId, Take, Page));
        }
        #endregion

        #region search

        [HttpGet("SearchClinics")]
        public async Task<IActionResult> SearchClinics([FromQuery] string Search, [FromQuery] int Take = 0, [FromQuery] int Page = 0)
        {
            return Ok(await docServ.SearchClinicsAsync(Search, Take, Page));
        }
        #endregion

        #region get ByID 
        [HttpGet("GetClinic")]
        public async Task<IActionResult> GetClinic([FromQuery] int Id) => Ok(await docServ.GetClininByIdAsync(Id));


        #endregion

        #region Check Clinic 
        [HttpPost("CheckClinic")]
        public async Task<IActionResult> CheckClinic([FromQuery]int ClinicId, [FromQuery] int d, [FromQuery] int m, [FromQuery] int y, [FromQuery] bool IsReCheck)
        {
            var UserName = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok( await docServ.CheckThisClinicAsync(ClinicId, UserName, d, m, y, IsReCheck));
        }
        #endregion

        #region Finish Check 
        [HttpPost("FinishCheck")]
        public async Task<IActionResult> FinishCheck([FromQuery] int Id) => Ok(await docServ.FinishCheckAsync(Id));

        #endregion
        #endregion




    }
}
