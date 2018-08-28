using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using NotepadAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<controller>
        [HttpGet]
        public IActionResult Get()
        {
            int id = 1;
            return Ok();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                User currentUser = contextDb.Users.FirstOrDefault(u => u.UserId == id);
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

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody] User newUser)
        {
            try
            {
                User userForCheck = contextDb.Users.FirstOrDefault(u => u.Email == newUser.Email);
                
                if ((userForCheck == null)&&(newUser.Email!=null))
                {
                    contextDb.Users.Add(newUser);
                    contextDb.SaveChanges();

                    return Ok(newUser);
                }
                return BadRequest();
            }
            catch(Exception e)
            {
                return StatusCode(500);
            }

        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody]User updatedUser)
        {
            try
            {
                User currentUser = contextDb.Users.FirstOrDefault(u => u.UserId ==id);
                if (currentUser != null)
                {

                    
                    contextDb.Users.Update(updatedUser);
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
    

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                User currentUser = contextDb.Users.FirstOrDefault(u => u.UserId == id);
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
