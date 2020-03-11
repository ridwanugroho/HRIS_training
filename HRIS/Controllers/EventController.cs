using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using HRIS.Data;
using HRIS.Models;

namespace HRIS.Controllers
{
    public class EventController : Controller
    {
        private AppDbContext db;

        public EventController(AppDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var events = (from e in db.Event select e).ToList();

            ViewData["events"] = events;

            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateEvent(Event evt)
        {
            evt.CreatedAt = DateTime.Now;
            evt.Status = "undone";

            db.Event.Add(evt);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}