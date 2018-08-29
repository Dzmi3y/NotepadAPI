using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotepadAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace NotepadAPI.Controllers
{
    [Route("api/[controller]")]
    public class EntryController : Controller
    {
        private IApplicationContext contextDb;

        public EntryController(IApplicationContext contextDb)
        {
            this.contextDb = contextDb;
        }

        [HttpGet,Authorize]
        public IActionResult Get()
        {
            try
            {
                Claim claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string idOfCurrentUser = claim.Value;

                List<Entry> entries = contextDb.Entries.Where(e => e.UserId.ToString() == idOfCurrentUser).ToList();
                return Ok(entries);

            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpGet("{id}"), Authorize]
        public IActionResult Get(int id)
        {
            try
            {
                Claim claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string idOfCurrentUser = claim.Value;

                Entry entry = contextDb.Entries.FirstOrDefault(e => (e.UserId.ToString() == idOfCurrentUser) && (e.EntryId == id));
                if (entry == null)
                {
                    return BadRequest();
                }
                return Ok(entry);

            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpPost, Authorize]
        public IActionResult Post([FromBody]Entry newEntry)
        {
            try
            {
                Claim claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                int idOfCurrentUser =Convert.ToInt32( claim.Value);

                
                if (newEntry == null)
                {
                    return BadRequest();
                }

                newEntry.UserId = idOfCurrentUser;
                contextDb.Entries.Add(newEntry);
                contextDb.SaveChanges();

                return Ok(newEntry);

            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpPut("{id}"), Authorize]
        public IActionResult Put(int id, [FromBody]Entry updateEntry)
        {
            try
            {
                Claim claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string idOfCurrentUser = claim.Value;

                Entry entry = contextDb.Entries.FirstOrDefault(e => (e.UserId.ToString() == idOfCurrentUser) && (e.EntryId == id));
                if (entry == null)
                {
                    return BadRequest();
                }

                entry.Text = updateEntry.Text;
                entry.Title = updateEntry.Title;
                contextDb.Entries.Update(entry);
                contextDb.SaveChanges();

                return Ok(entry);

            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpDelete("{id}"), Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                Claim claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                string idOfCurrentUser = claim.Value;

                Entry entry = contextDb.Entries.FirstOrDefault(e => (e.UserId.ToString() == idOfCurrentUser) && (e.EntryId == id));
                if (entry == null)
                {
                    return BadRequest();
                }

                contextDb.Entries.Remove(entry);
                contextDb.SaveChanges();

                return Ok(entry);

            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}
