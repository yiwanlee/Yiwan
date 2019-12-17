using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SampleCoreNull.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleCoreNull.Controllers
{
    [Authorize]
    public class HomeController : BasicController
    {
        protected readonly CoreDbContext db;
        public HomeController(CoreDbContext dbContext)
        {
            db = dbContext;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.CtxId = db.ContextId.ToString();

            var model = new HomePageViewModel();

            SQLEmployeeData sqlData = new SQLEmployeeData(db);
            model.Employees = sqlData.GetAll();
            return View(model);
        }

        public ActionResult Detail(int id)
        {
            ViewBag.CtxId = db.ContextId.ToString();
            SQLEmployeeData sqlData = new SQLEmployeeData(db);

            Employee employee = sqlData.Get(id);

            return View(employee);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string name)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee();
                employee.Name = name;

                SQLEmployeeData sqlData = new SQLEmployeeData(db);
                sqlData.Add(employee);
                return RedirectToAction("Detail", new { id = employee.ID });
            }
            return View();
        }
    }

    public class SQLEmployeeData
    {
        private CoreDbContext _context { get; set; }

        public SQLEmployeeData(CoreDbContext context)
        {
            _context = context;
        }

        public void Add(Employee emp)
        {
            _context.Add(emp);
            _context.SaveChanges();
        }

        public Employee Get(int ID)
        {
            return _context.Employee.FirstOrDefault(e => e.ID == ID);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employee.ToList<Employee>();
        }
    }

    public class HomePageViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
    }
}
