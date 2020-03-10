using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using HRIS.Models;
using HRIS.Data;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace HRIS.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicantController : Controller
    {
        private AppDbContext db;


        public ApplicantController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Show");
        }

        [HttpGet("show")]
        public IActionResult Show(int? perpage, int? page, int? order, string filter, int? status)
        {
            ViewData["filters"] = new { perpage, page, order, filter, status };

            var apls = (from e in db.Applicant where e.DataStatus != 0 select e).ToList();

            if (!string.IsNullOrEmpty(filter) || !string.IsNullOrWhiteSpace(filter))
            {
                apls = (from i in apls
                        where i.FullName.Contains(filter) ||
                                i.Role.Name.Contains(filter) ||
                                i.Role.Division.Contains(filter) ||
                                i.Role.SubDivision.Contains(filter)
                        select i).ToList();
            }

            if (order != null)
            {
                apls = orderBy(apls, order);
            }

            if (status.HasValue)
                apls = (from e in apls where e.Role.Status == status.Value select e).ToList();

            ViewBag.order = order;
            ViewBag.filter = filter;
            ViewBag.perPage = perpage;

            int _perPage = 5;
            if (perpage.HasValue)
                _perPage = perpage.Value;

            var pager = new Pager(apls.Count(), page, _perPage);

            var viewModel = new ApplicantPager
            {
                Items = apls.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };


            ViewData["Applicants"] = apls.ToList<Applicant>();
            

            return View(viewModel);
        }



        [HttpGet("add")]
        public IActionResult AddApplicant()
        {
            //return add Applicant page
            return View();
        }

        [HttpGet("edit/{id}")]
        public IActionResult Edit(string id)
        {
            var emp = db.Applicant.Find(Guid.Parse(id));

            ViewData["editData"] = emp;

            return View("AddApplicant");
        }

        [HttpPost("submit")]
        public IActionResult AddEmpoyee(Applicant emp)
        {
            //nanti ditambah validasi Applicant data
            emp.CreatedAt = DateTime.Now;
            emp.DataStatus = 1;
            db.Applicant.Add(emp);
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


        [HttpPost("UploadCV")]
        public async Task<IActionResult> UploadCV([FromForm(Name = "file")] IFormFile files)
        {
            var path = "wwwroot/doc/";
            var filename = "";
            Directory.CreateDirectory(path);
            if (files != null)
            {
                Console.WriteLine("ada filenya : {0}", files.FileName);

                filename = Path.Combine(path, Path.GetRandomFileName() + ".pdf");
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
            var emp = db.Applicant.Find(Guid.Parse(id));
            ViewData["Applicant"] = emp;

            //return View();
            return Ok(emp);
        }

        [HttpPost("Update")]
        public IActionResult Update(Applicant emp)
        {
            // validasi data
            emp.DataStatus = 1;
            //validasi data
            var _emp = db.Applicant.Find(emp.Id);
            var propName = typeof(Applicant).GetProperties();

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

        [HttpGet("Remove/{id}")]
        public IActionResult Remove(string id)
        {
            var _emp = db.Applicant.Find(Guid.Parse(id));

            _emp.DeletedAt = DateTime.Now;
            _emp.DataStatus = 0;

            db.SaveChanges();

            return Ok(_emp);
        }



        public static List<Applicant> orderBy(List<Applicant> apls, int? order)
        {
            switch (order)
            {
                case 1:
                    apls = apls.OrderBy(p => p.FullName).ToList();
                    break;

                case 2:
                    apls = apls.OrderByDescending(p => p.FullName).ToList();
                    break;

                case 3:
                    apls = apls.OrderBy(p => p.Role.Division).ToList();
                    break;

                case 4:
                    apls = apls.OrderByDescending(p => p.Role.SubDivision).ToList();
                    break;

                case 5:
                    apls = apls.OrderBy(p => p.JoinDate).ToList();
                    break;

                case 6:
                    apls = apls.OrderByDescending(p => p.JoinDate).ToList();
                    break;
            }

            return apls;
        }
        public class ApplicantPager
        {
            public IEnumerable<Applicant> Items { get; set; }
            public Pager Pager { get; set; }
        }
    }
}