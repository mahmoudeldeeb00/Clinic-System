using Clinic_System.BL.IRepsitory;
using Clinic_System.DAL.Entities;
using Clinic_System.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _idntitysrv;
        public IdentityController(IIdentityService idntitysrv)
        {
        this._idntitysrv = idntitysrv;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromForm]RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _idntitysrv.RegisterAsync(model);
            return result.IsAuthenticated ? Ok(result) : BadRequest(result.Message);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromForm] LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _idntitysrv.LoginAsync(model);
            return result.IsAuthenticated ? Ok(result) : BadRequest(result.Message);
        }



    }
}
