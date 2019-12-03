using Microsoft.AspNetCore.Mvc;
using SampleCoreNull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleCoreNull.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var employee = new Employee { ID = 1, Name = "语飞" };
            var list = new List<Employee>();
            for (int i = 0; i < 10; i++) list.Add(employee);

            return new OkObjectResult(new { obj = employee, list = list, name = new { title = "other", employee } });
            //Content("Hello MVC! This Message Is Action With Used ContentResult");
        }

    }
}
