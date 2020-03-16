using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

using HRIS.Models;
using HRIS.Data;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace HRIS.Controllers
{
    //[Route("[controller]")]
    //[ApiController]
    public class EmployeeController : Controller
    {
        private AppDbContext db;


        public EmployeeController(AppDbContext db)
        {
            this.db = db;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Show");
        }

        [Authorize]
        //[HttpGet("show")]
        public IActionResult Show(int ? perpage, int ? page, int ? order, string filter, int ? status)
        {

            var emps = (from e in db.Employee where e.DataStatus != 0 select e).ToList();

            if (!string.IsNullOrEmpty(filter) || !string.IsNullOrWhiteSpace(filter))
            {
                emps = (from i in emps where    i.FullName.Contains(filter) || 
                                               i.Role.Name.Contains(filter) ||
                                               i.Role.Division.Contains(filter) ||
                                               i.Role.SubDivision.Contains(filter)
                                               select i).ToList();
            }

            if (order != null)
            {
                emps = orderBy(emps, order);
            }

            if (status.HasValue)
                emps = (from e in emps where e.Role.Status == status select e).ToList();

            ViewBag.page = page;
            ViewBag.status = status;
            ViewBag.order = order;
            ViewBag.filter = filter;
            ViewBag.perPage = perpage;

            int _perPage = 5;
            if (perpage.HasValue)
                _perPage = perpage.Value;

            var pager = new Pager(emps.Count(), page, _perPage);

            var viewModel = new IndexViewModel
            {
                Items = emps.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };


            //ViewData["employees"] = emps.ToList<Employee>();
            ViewData["employees"] = viewModel;

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Add()
        {
            //return add employee page
            return View();
        }

        [Authorize]
        //[HttpGet("edit/{id}")]
        public IActionResult Edit(string id)
        {
            var emp = db.Employee.Find(Guid.Parse(id));

            ViewData["editData"] = emp;

            return View("Add");
        }

        [Authorize]
        //[HttpPost("submit")]
        public async Task<IActionResult> Submit(string edit, Employee emp, Role role, Address addr, [FromForm(Name = "file")] IFormFile files)
        {
            if(files != null)
            {
                Console.WriteLine("apakah ada filenya? : {0}", files.FileName);
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

                emp.Photo = filename.Substring(8);
            }

            if(string.IsNullOrEmpty(edit))
            {
                //nanti ditambah validasi employee data
                emp.CreatedAt = DateTime.Now;
                emp.DataStatus = 1;
                emp.Role = role;
                emp.Address = addr;
                db.Employee.Add(emp);
                db.SaveChanges();

                return RedirectToAction("Show");
            }

            else
            {
                emp.Address = addr;
                emp.Role = role;
                return Update(emp);
            }
        }

        [Authorize]
        //[HttpPost("UploadPhoto")]
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

        [Authorize]
        //[HttpGet("Detail/{id}")]
        public IActionResult Detail(string id)
        {
            var emp = db.Employee.Find(Guid.Parse(id));
            ViewData["employee"] = emp;

            //return View();
            return Ok(emp);
        }

        [Authorize]
        //[HttpPost("Update")]
        public IActionResult Update(Employee emp)
        {
            // validasi data
            emp.DataStatus = 1;
            //validasi data
            var _emp = db.Employee.Find(emp.Id);
            var propName = typeof(Employee).GetProperties();

            emp.CreatedAt = _emp.CreatedAt;

            foreach (var n in propName)
            {
                Console.WriteLine("set : {0}", n.ToString());
                var val = n.GetValue(emp, null);
                n.SetValue(_emp, val);
            }

            _emp.EditedAt = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Show");
            //return Ok(_emp);
        }

        [Authorize]
        //[HttpGet("sendMessage")]
        public IActionResult SendMessage(string id, string message)
        {
            var addr = db.Employee.Find(Guid.Parse(id.Substring(2))).Email;

            MailController.sendMail("admin@tokoaneh.com", addr, "HR-Message", message);

            return RedirectToAction("Show");
        }

        [Authorize]
        //[HttpGet("RemoveEmployee/{id}")]
        public IActionResult RemoveEmployee(string id)
        {
            var _emp = db.Employee.Find(Guid.Parse(id));

            ViewData["remove"] = _emp;

            return View();
        }

        [Authorize]
        //[HttpGet("Remove")]
        public IActionResult Remove(string id, int status, string notes)
        {
            Console.WriteLine(id);
            Console.WriteLine(status);
            Console.WriteLine(notes);

            var _emp = db.Employee.Find(Guid.Parse(id));

            _emp.Role.Status = status;
            _emp.DeletedAt = DateTime.Now;
            _emp.DataStatus = 0;

            db.SaveChanges();

            return RedirectToAction("Show");
        }


        public static List<Employee> orderBy(List<Employee> emps, int? order)
        {
            switch (order)
            {
                case 1:
                    emps = emps.OrderBy(p => p.FullName).ToList();
                    break;

                case 2:
                    emps = emps.OrderByDescending(p => p.FullName).ToList();
                    break;

                case 3:
                    emps = emps.OrderBy(p => p.Role.Division).ToList();
                    break;

                case 4:
                    emps = emps.OrderByDescending(p => p.Role.SubDivision).ToList();
                    break;

                case 5:
                    emps = emps.OrderBy(p => p.JoinDate).ToList();
                    break;

                case 6:
                    emps = emps.OrderByDescending(p => p.JoinDate).ToList();
                    break;
            }

            return emps;
        }
    }
}