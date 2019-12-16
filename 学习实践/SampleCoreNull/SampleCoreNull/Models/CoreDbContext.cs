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
        public CoreDbContext()
        {
            //var opts = new DbContextOptionsBuilder<CoreDbContext>(new DbContextOptions<CoreDbContext>()).UseSqlite(AppSettings.AppSetting("sqlite")).Options;
        }

        public CoreDbContext(DbContextOptions<CoreDbContext> options)
            : base(options)
        {

        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=blogging.db");
        //}

        public DbSet<Employee> Employee { get; set; }
    }
}
