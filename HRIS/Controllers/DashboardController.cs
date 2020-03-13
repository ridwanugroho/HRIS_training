using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using HRIS.Models;
using HRIS.Data;

namespace HRIS.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext db;
        
        public DashboardController(AppDbContext db)
        {
            this.db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            var dashboardView = new DashboardViewData();

            var emps = (from e in db.Employee where e.DataStatus != 0 select e).ToList();
            var empReqs = (from e in db.EmployeeRequest select e).ToList();
            var applicants = (from a in db.Applicant select a).ToList();
            var dtNow = DateTime.Now.Date;

            dashboardView.TotalEMployee = emps.Count();
            dashboardView.MaleEmployee = (from e in emps where e.Gender == "male" select e).Count();
            dashboardView.FemaleEmployee = (from e in emps where e.Gender == "female" select e).Count();
            dashboardView.Absent = from e in empReqs
                                    where e.ApprovalStatus == 1 &&
                                    dtNow >= e.DueDate.StartDate.Date &&
                                    dtNow <= e.DueDate.EndDate.Date
                                    select (from x in db.Employee where x.Id.ToString() == e.From select x).First();
            
            dashboardView.TodayPresent = (from e in db.Attendance
                                          where e.ClockIn.Value.Date == dtNow
                                          select e).Count();

            var temp = (from e in db.Attendance
                        where e.ClockIn.Value.Date == dtNow
                        select e);
            temp.ToList().ForEach(x => Console.WriteLine(x.UserId));
            dashboardView.UpcomingEvent = (from e in db.Event where dtNow.AddDays(30) > e.StartDate
                                         orderby e.StartDate select e).ToList();
            dashboardView.NewApplicant = (from a in applicants where a.Role.Status == 1 select a).ToList();

            ViewData["dashboardView"] = dashboardView;

            return View();
        }
    }

    public class DashboardViewData
    {
        public int TotalEMployee { get; set; }
        public int MaleEmployee { get; set; }
        public int FemaleEmployee { get; set; }
        public IEnumerable<Employee> Absent { get; set; }
        public int TodayPresent { get; set; }
        public IEnumerable<Event> UpcomingEvent { get; set; }
        public IEnumerable<Applicant> NewApplicant { get; set; }
    }
}