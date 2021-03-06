﻿using System;
using System.Web;
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
using Microsoft.Extensions.FileProviders;

namespace HRIS.Controllers
{
    public class ApplicantIOController : Controller
    {
        private AppDbContext db;


        public ApplicantIOController(AppDbContext db)
        {
            this.db = db;
        }

        public FileResult GenereateAllApplicant()
        {
            var apls = from e in db.Applicant where e.DataStatus != 0 select e;

            return Download(apls.ToList());
        }

        public FileResult PartialDownload(int? perpage, int? page, int? order, string filter, int? status)
        {
            Console.WriteLine("download partial data");

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
                apls = ApplicantController.orderBy(apls, order);
            }

            if (status.HasValue)
                apls = (from e in apls where e.Role.Status == status select e).ToList();

            ViewBag.order = order;
            ViewBag.filter = filter;
            ViewBag.perPage = perpage;

            int _perPage = 5;
            if (perpage.HasValue)
                _perPage = perpage.Value;

            var pager = new Pager(apls.Count(), page, _perPage);

            var viewModel = new ApplicantController.ApplicantPager
            {
                Items = apls.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            foreach (var e in viewModel.Items)
                Console.WriteLine(e.FullName);

            return Download(viewModel.Items.ToList());
        }

        public FileResult Download(List<Applicant> apls)
        {
            var sb = new StringBuilder();
            var nameProp = typeof(Applicant).GetProperties();

            foreach (var e in apls)
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

            return File(new UTF8Encoding().GetBytes(sb.ToString()), "text/csv", "Applicant.csv");
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

                    var toAdd = new Applicant()
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

                    db.Applicant.Add(toAdd);
                }

                db.SaveChanges();

                return RedirectToAction("Show", "Applicant");
            }
            catch (System.Exception e)
            {

                return Ok("format salah\n\n" + e);
            }
        }

        public FileResult downloadCV(string cvPath)
        {
            string filePath = "d:\\Prog\\HRIS\\HRIS\\wwwroot\\";
            IFileProvider provider = new PhysicalFileProvider(filePath);
            IFileInfo fileInfo = provider.GetFileInfo(cvPath);
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "application/pdf";
            return File(readStream, mimeType, cvPath);
        }

        public FileResult ShowCV(string cvPath)
        {
            var cd = new System.Net.Mime.ContentDisposition()
            {
                FileName = "CV-"+cvPath,
                Inline = true
            };
            cd.FileName = "CV-" + cvPath;
            
            string filePath = "d:\\Prog\\HRIS\\HRIS\\wwwroot\\" + cvPath;
            var mimeType = "application/pdf";
            HttpContext.Response.Headers.Append("Content-Disposition", cd.ToString());
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            return File(System.IO.File.ReadAllBytes(filePath), mimeType);
        }

        public FileResult downloadTemplate()
        {
            string fileName = "applicantTemplate.csv";
            string filePath = "d:\\Prog\\HRIS\\HRIS\\wwwroot\\doc\\";
            IFileProvider provider = new PhysicalFileProvider(filePath);
            IFileInfo fileInfo = provider.GetFileInfo(fileName);
            var readStream = fileInfo.CreateReadStream();
            var mimeType = "text/scv";
            return File(readStream, mimeType, fileName);
        }
    }
}