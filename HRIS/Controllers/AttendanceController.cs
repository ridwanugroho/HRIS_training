using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using HRIS.Models;
using HRIS.Data;

namespace HRIS.Controllers
{
    public class AttendanceController : Controller
    {
        private AppDbContext db;

        public AttendanceController(AppDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var attn = (from a in db.Employee select a).ToList();

            ViewData["attendances"] = attn;

            return View();
        }

        public IActionResult Detail(string id)
        {
            var username = db.Employee.Find(Guid.Parse(id)).FullName;
            var attn = (from a in db.Attendance where a.UserId == id select a).ToList();

            attn.ForEach(x => x.UserId = username);
            ViewData["attnName"] = username;
            ViewData["detail"] = attn;
            
            return View();
        }

        public IActionResult GetAttendanceByTime(string timeScope, string value)
        {
            return Ok();
        }



        //Input Attendance
        public IActionResult ClockIn(string id, string date, string dueDate)
        {
            var _date = new DateTime();
            var _dueDate = new DateTime();

            if (!string.IsNullOrEmpty(dueDate) && !string.IsNullOrEmpty(date))
            {
                _date = DateTime.Parse(date);
                _dueDate = DateTime.Parse(dueDate);
                Console.WriteLine(_date);
                Console.WriteLine(_dueDate);
            }

            else
            {
                _date = DateTime.Now;
                _dueDate = DateTime.Now.Date;
            }

            var userAttendanceToday = from a in db.Attendance
                                      where a.UserId == id &&
                                            a.CreatedAt.Date == _dueDate.Date
                                            select a;

            if (userAttendanceToday.Count() == 0)
            {
                var now = _dueDate.Date.ToString("yyyy/MM/dd") + " 08:00:00.000";

                var clockInStatus = "Ok";
                var clockIn = _date;

                if (clockIn > DateTime.Parse(now))
                    clockInStatus = "L";

                var _attn = new Attendance()
                {
                    UserId = id,
                    ClockIn = clockIn,
                    CreatedAt = clockIn,
                    ClockInStatus = clockInStatus,
                    Status = "A1"

                };

                db.Attendance.Add(_attn);
                db.SaveChanges();

                return Ok(_attn);
            }

            else
            {
                if (userAttendanceToday.First().ClockOut != null)
                    return BadRequest("ERROR");

                var _attn = db.Attendance.Find(userAttendanceToday.First().Id);

                var now = DateTime.Today.ToString("yyyy/MM/dd") + " 17:00:00.000";
                var clockOutStatus = "Ok";
                var clockOut = _date;

                if (clockOut < DateTime.Parse(now))
                    clockOutStatus = "U";

                if (clockOut > DateTime.Parse(now))
                    clockOutStatus = "O";

                _attn.ClockOut = clockOut;
                _attn.EditedAt = clockOut;
                _attn.ClockOutStatus = clockOutStatus;
                _attn.Status = "Ok";

                db.SaveChanges();

                return Ok(_attn);
            }

        }
    }
}