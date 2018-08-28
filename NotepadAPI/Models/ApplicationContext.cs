using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NotepadAPI.Models
{
    public class ApplicationContext:DbContext, IApplicationContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        { }

       public DbSet<Entry> Entries { get; set; }
       public DbSet<User> Users { get; set; }
        
    }
}
