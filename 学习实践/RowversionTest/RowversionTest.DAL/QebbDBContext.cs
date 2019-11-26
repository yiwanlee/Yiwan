using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Reflection;

namespace RowversionTest.DAL
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;

    public partial class QebbDBContext : DbContext
    {
        public QebbDBContext() : base("name=qebb")
        {
            this.Database.CommandTimeout = 600000; //时间单位是毫秒
        }

        public virtual DbSet<GamDogluckyrec> GamDogluckyrec { get; set; }
    }
}
