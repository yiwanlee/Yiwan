using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleCoreNull.Controllers
{
    public class BasicController : Controller
    {
        protected readonly CoreDbContext _context;

        protected BasicController(CoreDbContext context) { _context = context; }
    }
}
