using Clinic_System.BL.IRepsitory;
using Clinic_System.DAL.Entities;
using Clinic_System.DTOS;
using Clinic_System.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Clinic_System.BL.Repository
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _usermng;
        public JWT _jwt;
        public IdentityService(UserManager<AppUser> usermng, IOptions<JWT> jwt)
        {
            this._usermng = usermng;
            this._jwt = jwt.Value;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            var userClaims = await _usermng.GetClaimsAsync(user);
            var roles = await _usermng.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Isseur,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.ExpireInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<AuthenticationModel> RegisterAsync(RegisterDTO model)
        {
            if (await _usermng.FindByEmailAsync(model.Email) is not null)
                return new AuthenticationModel { Message = "this Email is already registered " };

            if (await _usermng.FindByNameAsync(model.UserName) is not null)
                return new AuthenticationModel { Message = "this User Name is already registered " };

            var user = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
                CityId = model.CityId,
                GenderId = model.GenderId
            };
            var result = await _usermng.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                {
                    errors += error.Description + " ";
                }
                return new AuthenticationModel { Message = errors };
            }
            await _usermng.AddToRoleAsync(user, "User");
            var JwtSecurityToken = await CreateJwtToken(user);
            return new AuthenticationModel
            {
                Email = user.Email,
                ExpiredOn = JwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken),
                UserName = user.UserName
            };
        }

        public async Task<AuthenticationModel> LoginAsync(LoginDTO model)
        {
            var AuthModel = new AuthenticationModel();
            var user = await _usermng.FindByEmailAsync(model.EmailOrUserName);
            if(user == null)
                user = await _usermng.FindByNameAsync(model.EmailOrUserName);

            if (user is null || !await _usermng.CheckPasswordAsync(user, model.Password))
            {
                AuthModel.Message = "email or password is in correct ";
                return AuthModel;
            }
            var JwtSecurityToken = await CreateJwtToken(user);
            var roles = await _usermng.GetRolesAsync(user);

            AuthModel.IsAuthenticated = true;
            AuthModel.Token = new JwtSecurityTokenHandler().WriteToken(JwtSecurityToken);
            AuthModel.Email = user.Email;
            AuthModel.UserName = user.UserName;
            AuthModel.ExpiredOn = JwtSecurityToken.ValidTo;
            AuthModel.Roles = roles.ToList();





            return AuthModel;
        }
    }
}
