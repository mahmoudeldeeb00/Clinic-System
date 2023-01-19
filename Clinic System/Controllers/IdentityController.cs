using Clinic_System.BL.IRepsitory;
using Clinic_System.DAL.Entities;
using Clinic_System.DTOS;
using Clinic_System.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _idntitysrv.RegisterAsync(model);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _idntitysrv.LoginAsync(model);
            return Ok(result);
        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public IActionResult getCurrentUserInfromation()
        {
            try
            {
                var UserName = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var x = _idntitysrv.getuserinfo(UserName);
                return Ok(x);
            }
            catch
            {
                return Ok(new Response<AppUser> { message = " an error ocuurs "});
            }

        }

    }
}
