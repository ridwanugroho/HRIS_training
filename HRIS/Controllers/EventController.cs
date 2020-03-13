using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using HRIS.Data;
using HRIS.Models;
using Microsoft.AspNetCore.Authorization;

namespace HRIS.Controllers
{
    public class EventController : Controller
    {
        private AppDbContext db;

        public EventController(AppDbContext db)
        {
            this.db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            var events = (from e in db.Event where e.Status != "done" select e).ToList();

            ViewData["events"] = events;

            return View();
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        public IActionResult CreateEvent(Event evt)
        {
            evt.CreatedAt = DateTime.Now;
            evt.Status = "undone";

            db.Event.Add(evt);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Edit(string Id)
        {
            var evt = db.Event.Find(Guid.Parse(Id));

            Console.WriteLine(evt.Title);

            ViewData["edit"] = evt;

            return View("Create");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Update(Event evt)
        {
            
            var _evt = db.Event.Find(evt.Id);
            var propName = typeof(Event).GetProperties();

            evt.CreatedAt = _evt.CreatedAt;

            foreach (var n in propName)
            {
                Console.WriteLine("set : {0}", n.ToString());
                var val = n.GetValue(evt, null);
                n.SetValue(_evt, val);
            }

            _evt.EditedAt = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Done(string id)
        {
            var evt = db.Event.Find(Guid.Parse(id));

            evt.Status = "done";

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}