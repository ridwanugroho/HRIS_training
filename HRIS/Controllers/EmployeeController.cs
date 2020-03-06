using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using HRIS.Models;
using HRIS.Data;
using System.IO;

namespace HRIS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private AppDbContext db;


        public EmployeeController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("add")]
        public IActionResult AddEmployee()
        {
            //return add employee page
            return View();
        }

        [HttpPost("submit")]
        public IActionResult AddEmpoyee(Employee emp)
        {
            //nanti ditambah validasi employee data
            db.Employee.Add(emp);
            db.SaveChanges();

            return Ok(emp);
        }

        [HttpPost("UploadPhoto")]
        public async Task<IActionResult> UploadPhoto([FromForm(Name = "file")] IFormFile files)
        {
            var path = "wwwroot/img/usr_img/";
            var filename = "";
            Directory.CreateDirectory(path);
            if (files != null)
            {
                Console.WriteLine("ada gambarnya : {0}", files.FileName);

                filename = Path.Combine(path, Path.GetRandomFileName() + ".jpg");
                using (var stream = new FileStream(filename, FileMode.Create))
                {
                    await files.CopyToAsync(stream);
                }
            }

            return Ok(filename.Substring(8));
        }

        [HttpGet("Detail/{id}")]
        public IActionResult Detail(string id)
        {
            var emp = db.Employee.Find(Guid.Parse(id));
            ViewData["employee"] = emp;
            
            return View();
        }

        [HttpGet("show")]
        public IActionResult Show()
        {
            var emps = from e in db.Employee select e;
            ViewData["employees"] = emps.ToList<Employee>();

            return View();
        }
    }
}