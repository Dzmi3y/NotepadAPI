using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotepadAPI.Models
{
    public class Entry
    {
        public int EntryId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
