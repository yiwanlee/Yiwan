using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleCoreNull.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SampleCoreNull
{
    public class CoreDbContext : IdentityDbContext<User>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=blogging.db");
        }

        public DbSet<Employee> Employee { get; set; }
    }
}
