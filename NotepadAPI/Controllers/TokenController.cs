using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using NotepadAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using System.Text;


namespace NotepadAPI.Controllers
{
    [Route("api/GetToken")]
    public class TokenController : Controller
    {
        private IConfiguration config;
        private IApplicationContext contextDb;
        public TokenController(IConfiguration config, IApplicationContext contextDb)
        {
            this.config = config;
            this.contextDb = contextDb;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserDataForAuthentication dataForAuthentication)
        {
            IActionResult response = Unauthorized();

            User currentUser = contextDb.Users.FirstOrDefault(u => ((u.Email == dataForAuthentication.Email) && (u.Password == dataForAuthentication.Password)));

            if (currentUser != null)
            {
                var tokenString = BuildToken(currentUser.UserId);
                response = Ok(new { token = tokenString, email=currentUser.Email});
            }
            return response;
        }

        private string BuildToken(int userId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId,userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
