using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NotepadAPI.Models
{
    public interface IApplicationContext
    {
        DbSet<Entry> Entries { get; set; }
        DbSet<User> Users { get; set; }
        int SaveChanges();
    }
}
