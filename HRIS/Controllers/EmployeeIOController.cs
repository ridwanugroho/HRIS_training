using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using HRIS.Models;
using HRIS.Data;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace HRIS.Controllers
{
    public class EmployeeIOController : Controller
    {
        private AppDbContext db;


        public EmployeeIOController(AppDbContext db)
        {
            this.db = db;
        }

        public FileResult GenereateAllEmployee()
        {
            var emps = from e in db.Employee where e.DataStatus != 0 select e;

            return Download(emps.ToList());
        }
        
        public FileResult PartialDownload(int? perpage, int? page, int? order, string filter, int? status)
        {
            Console.WriteLine("download partial data");

            var emps = (from e in db.Employee where e.DataStatus != 0 select e).ToList();

            if (!string.IsNullOrEmpty(filter) || !string.IsNullOrWhiteSpace(filter))
            {
                emps = (from i in emps
                        where i.FullName.Contains(filter) ||
                                i.Role.Name.Contains(filter) ||
                                i.Role.Division.Contains(filter) ||
                                i.Role.SubDivision.Contains(filter)
                        select i).ToList();
            }

            if (order != null)
            {
                emps = EmployeeController.orderBy(emps, order);
            }

            if (status.HasValue)
                emps = (from e in emps where e.Role.Status == status select e).ToList();

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

            foreach (var e in viewModel.Items)
                Console.WriteLine(e.FullName);

            return Download(viewModel.Items.ToList());
        }

        public FileResult Download(List<Employee> emps)
        {
            var sb = new StringBuilder();
            var nameProp = typeof(Employee).GetProperties();

            foreach(var e in emps)
            {
                sb.AppendLine(
                    e.Id + "," +
                    e.NIK + "," +
                    e.FullName + "," +
                    e.DOB + "," +
                    e.Religion + "," +
                    e.JoinDate + "," +
                    e.Phone + "," +
                    e.Address.Country + "," +
                    e.Address.Province + "," +
                    e.Address.City + "," +
                    e.Address.Distric + "," +
                    e.Address.SubDistric + "," +
                    e.Address.Street + "," +
                    e.Role.Status + "," +
                    e.Role.Name + "," +
                    e.Role.Division + "," +
                    e.Role.SubDivision + "," +
                    e.Role.Level + "," +
                    e.Role.Description
                    );
            }

            return File(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv", "employee.csv");
        }

        public IActionResult Import([FromForm(Name = "files")] IFormFile files)
        {
            try
            {
                var streamer = new StreamReader(files.OpenReadStream());
                var str = streamer.ReadToEnd();
                var strLines = str.Split('\n');

                foreach (var line in strLines.Skip(2).Take(strLines.Count() - 3))
                {
                    Console.WriteLine(line);
                    var singleData = line.Split(',');
                    var _address = new Address()
                    {
                        Country = singleData[6],
                        Province = singleData[7],
                        City = singleData[8],
                        Distric = singleData[9],
                        SubDistric = singleData[10],
                        Street = singleData[11]
                    };

                    var _role = new Role()
                    {
                        Status = int.Parse(singleData[12]),
                        Name = singleData[13],
                        Division = singleData[14],
                        SubDivision = singleData[15],
                        Level = singleData[16],
                        Description = singleData[17]
                    };

                    var toAdd = new Employee()
                    {
                        NIK = singleData[0],
                        FullName = singleData[1],
                        DOB = DateTime.Parse(singleData[2]),
                        Religion = singleData[3],
                        JoinDate = DateTime.Parse(singleData[4]),
                        Phone = singleData[5],
                        Address = _address,
                        Role = _role,
                        DataStatus = 1,
                        CreatedAt = DateTime.Now
                    };

                    var raw = JsonConvert.SerializeObject(toAdd, Formatting.Indented);
                    Console.WriteLine(raw);

                    db.Employee.Add(toAdd);
                }

                db.SaveChanges();

                return RedirectToAction("Show", "Employee");
            }
            catch (System.Exception e)
            {

                return Ok("format salah\n\n" + e);
            }
        }
    }
}