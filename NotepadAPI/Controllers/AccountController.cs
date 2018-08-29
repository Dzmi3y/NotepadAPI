using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using NotepadAPI.Models;


namespace NotepadAPI.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private IApplicationContext contextDb;

        public AccountController(IApplicationContext contextDb)
        {
            this.contextDb = contextDb;
        }

        [HttpGet, Authorize]
        public IActionResult Get()
        {
            try
            {
                Claim claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string idOfCurrentUser = claim.Value;

                User currentUser = contextDb.Users.FirstOrDefault(u => u.UserId.ToString() == idOfCurrentUser);
                if (currentUser != null)
                {
                    return Ok(currentUser);
                }
                return BadRequest();
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] User newUser)
        {
            try
            {
                User userForCheck = contextDb.Users.FirstOrDefault(u => u.Email == newUser.Email);

                if ((userForCheck == null) && (newUser.Email != null))
                {
                    contextDb.Users.Add(newUser);
                    contextDb.SaveChanges();

                    return Ok(newUser);
                }
                return BadRequest();
            }
            catch 
            {
                return StatusCode(500);
            }

        }

        
        [HttpPut, Authorize]
        public IActionResult Put([FromBody]UserDataForAuthentication updatedAuthenticationData)
        {
            try
            {
                Claim claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string idOfCurrentUser = claim.Value;

                User currentUser = contextDb.Users.FirstOrDefault(u => u.UserId.ToString() == idOfCurrentUser);

                User checkedUser = contextDb.Users.FirstOrDefault(u => u.Email == updatedAuthenticationData.Email);

                if ((checkedUser != null) && (checkedUser != currentUser))
                {
                    return BadRequest("Email exists");
                }

                if ((currentUser != null) && (updatedAuthenticationData != null))
                {

                    currentUser.Email = updatedAuthenticationData.Email;

                    if ((updatedAuthenticationData.Password != "") && (updatedAuthenticationData.Password != null))
                    {
                        currentUser.Password = updatedAuthenticationData.Password;
                    }

                    contextDb.Users.Update(currentUser);
                    contextDb.SaveChanges();

                    return Ok(currentUser);
                }
                return BadRequest();
            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpDelete, Authorize]
        public IActionResult Delete()
        {
            try
            {
                Claim claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string idOfCurrentUser = claim.Value;


                User currentUser = contextDb.Users.FirstOrDefault(u => u.UserId.ToString() == idOfCurrentUser);
                if (currentUser != null)
                {
                    contextDb.Users.Remove(currentUser);
                    contextDb.SaveChanges();

                    return Ok(currentUser);
                }
                return BadRequest();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
