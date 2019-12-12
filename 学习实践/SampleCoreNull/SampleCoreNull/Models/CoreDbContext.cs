using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SampleCoreNull.Models;

namespace SampleCoreNull
{
    public class CoreDbContext : DbContext
    {
        public CoreDbContext()
        {
            var opts = new DbContextOptionsBuilder<CoreDbContext>(new DbContextOptions<CoreDbContext>()).UseSqlite(AppSettings.AppSetting("sqlite")).Options;
        }

        public CoreDbContext(DbContextOptions<CoreDbContext> options)
           : base(options)
        {

        }

        public DbSet<Employee> Employee { get; set; }
    }
}
