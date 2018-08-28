using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotepadAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Entry> Entries { get; set; }
    }
}
