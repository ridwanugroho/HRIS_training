using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using HRIS.Models;
using HRIS.Data;
using Microsoft.AspNetCore.Authorization;

namespace HRIS.Controllers
{
    public class AttendanceController : Controller
    {
        private AppDbContext db;

        public AttendanceController(AppDbContext db)
        {
            this.db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            var emps = (from a in db.Employee select a).ToList();
            var attnViewData = new List<AttendanceViewData>();

            foreach(var e in emps)
            {
                var attnDetail = from a in db.Attendance where a.UserId == e.Id.ToString() select a.Status;
                
                var attVD = new AttendanceViewData();
                attVD.Employee = e;
                attVD.AttendanceStatus = "Ok";

                foreach(var a in attnDetail)
                {
                    Console.WriteLine("cek status {0} : {1}", e, a);
                    if (a != "Ok")
                    {
                        attVD.AttendanceStatus = a;
                        attVD.Detail = "Contact HR";

                        break;
                    }
                }

                attnViewData.Add(attVD);
            }

            foreach (var i in attnViewData)
                Console.WriteLine("name: {0}    status:{1}", i.Employee.FullName, i.AttendanceStatus);

            ViewData["attendances"] = attnViewData;

            return View();
        }

        [Authorize]
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
            Console.WriteLine(id);
            Console.WriteLine(date);
            Console.WriteLine(dueDate);
            var _userID = (from e in db.Employee where e.NIK == id select e.Id.ToString()).First();
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
                                      where a.UserId == _userID &&
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
                    UserId = _userID,
                    ClockIn = clockIn,
                    CreatedAt = clockIn,
                    ClockInStatus = clockInStatus,
                    Status = "A1"

                };

                Console.WriteLine("create attendance today. ClockIn");
                db.Attendance.Add(_attn);
                db.SaveChanges();

                return Ok(_attn);
            }

            else
            {
                if (userAttendanceToday.First().ClockOut != null)
                    return BadRequest("ERROR");

                var _attn = db.Attendance.Find(userAttendanceToday.First().Id);
                _attn.Status = "Ok";

                var now = DateTime.Today.ToString("yyyy/MM/dd") + " 18:00:00.000";
                var clockOutStatus = "Ok";
                var clockOut = _date;

                if (clockOut < DateTime.Parse(now))
                {
                    clockOutStatus = "U";
                    _attn.Status = "U";
                }

                if (clockOut > DateTime.Parse(now))
                {
                    clockOutStatus = "O";
                    _attn.Status = "O";
                }

                _attn.ClockOut = clockOut;
                _attn.EditedAt = clockOut;
                _attn.ClockOutStatus = clockOutStatus;

                Console.WriteLine("ClockOut");
                db.SaveChanges();

                return Ok(_attn);
            }

        }
    
    }

    public class AttendanceViewData
    {
        public Employee Employee { get; set; }
        public string AttendanceStatus { get; set; }
        public string Detail { get; set; }
    }
}