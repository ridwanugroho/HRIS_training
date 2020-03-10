using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using HRIS.SignalR;
using HRIS.Models;
using HRIS.Data;

namespace HRIS.Controllers
{
    public class EmpRequestController : Controller
    {
        private AppDbContext db;
        private readonly IHubContext<ChatHub> _hubContext;

        public EmpRequestController(IHubContext<ChatHub> _hubContext, AppDbContext db)
        {
            this._hubContext = _hubContext;
            this.db = db;
        }

        public async Task<IActionResult> Index(string sender, string to, string subject, string message)
        {
            var _sender = (from e in db.Employee where e.NIK == sender select e).First();
            var _reciever = (from e in db.HRAdmin where e.Email == to select e).First();

            var msg = new NotificationLog
            {
                Sender = _sender.FullName,
                Subject = subject,
                Message = message,
                CreatedAt = DateTime.Now
            };

            foreach (var connectionId in ChatHub._connections.GetConnections(to))
                await _hubContext.Clients.Client(connectionId).SendAsync("GotMessage", msg);


            msg.Sender = _sender.Id.ToString();
            msg.Reciever = _reciever.Id.ToString();

            db.NotificationLog.Add(msg);
            db.SaveChanges();

            registerRequest(sender, to, subject, message);

            return Ok("sepertinya terkirim");
        }

        public IActionResult GetUnreadNotif(string Id)
        {
            return Ok();
        }

        public IActionResult SetReadStatus(string msgID)
        {
            var _msg = db.NotificationLog.Find(Guid.Parse(msgID));
            _msg.OpenedAt = DateTime.Now;
            db.SaveChanges();

            return Ok("read");
        }

        public IActionResult AllRequest(int? perpage, int? page, int? order, string filter, int? status)
        {
            var reqs = (from r in db.NotificationLog select r).ToList();

            if (!string.IsNullOrEmpty(filter) || !string.IsNullOrWhiteSpace(filter))
            {
                reqs = (from i in reqs
                        where i.Message.Contains(filter) ||
                                i.Subject.Contains(filter) ||
                                i.Sender.Contains(filter)
                        select i).ToList();
            }

            /*if (order != null)
            {
                reqs = orderBy(reqs, order);
            }*/

            //if (status.HasValue)
            //    reqs = (from e in reqs where e.Role.Status == status.Value select e).ToList();

            ViewBag.order = order;
            ViewBag.filter = filter;
            ViewBag.perPage = perpage;

            int _perPage = 5;
            if (perpage.HasValue)
                _perPage = perpage.Value;

            var pager = new Pager(reqs.Count(), page, _perPage);

            var viewModel = new RequestPager
            {
                Items = reqs.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };


            ViewData["Applicants"] = reqs.ToList<NotificationLog>();


            return View(viewModel);
        }

        private void registerRequest(string sender, string to, string subject, string message)
        {
            var _req = new EmployeeRequest
            {
                From = (from e in db.Employee where e.NIK == sender select e.Id.ToString()).First(),
                To = (from e in db.HRAdmin where e.Email == to select e.Id.ToString()).First(),
                Subject = subject,
                Message = message,
                ApprovalStatus = 0,
                CreatedAt = DateTime.Now,
                Notes = "-"
            };

            db.EmployeeRequest.Add(_req);
            db.SaveChanges();
        }

        public class RequestPager
        {
            public IEnumerable<NotificationLog> Items { get; set; }
            public Pager Pager { get; set; }
        }
    }
}