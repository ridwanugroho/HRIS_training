using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

using HRIS.SignalR;
using HRIS.Models;
using HRIS.Data;
using Microsoft.AspNet.SignalR;

namespace HRIS.Controllers
{
    public class EmpRequestController : Controller
    {
        private AppDbContext db;
        private readonly Microsoft.AspNetCore.SignalR.IHubContext<ChatHub> _hubContext;

        public EmpRequestController(Microsoft.AspNetCore.SignalR.IHubContext<ChatHub> _hubContext, AppDbContext db)
        {
            this._hubContext = _hubContext;
            this.db = db;
        }

        public async Task<IActionResult> Index(string sender, string to, string subject, string message, DateTime startDate, DateTime endDate)
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
                    
            msg.Reciever = _reciever.Id.ToString();
            msg.CreatedAt = DateTime.Now;

            db.NotificationLog.Add(msg);

            foreach (var connectionId in ChatHub._connections.GetConnections(to))
                await _hubContext.Clients.Client(connectionId).SendAsync("GotMessage", msg);
            
            msg.Sender = _sender.Id.ToString();
            db.SaveChanges();
            
            registerRequest(msg.Id.ToString(), sender, to, subject, message, startDate, endDate);

            return Ok("Permintaan anda berhasil dikirimkan");
        }

        public IActionResult GetUnreadNotif(string Id)
        {
            var unread = (from n in db.NotificationLog where n.Reciever == Id && n.ReadStatus != "read" select n).ToList();

            unread.ForEach(
                x => x.Sender = db.Employee.Find(Guid.Parse(x.Sender)).FullName
                );

            return Ok(unread);
        }

        public IActionResult Detail(string notifId, string Id)
        {
            var req = new EmployeeRequest();

            if (!string.IsNullOrEmpty(notifId))
            {
                req = (from r in db.EmployeeRequest where r.NotifId == notifId select r).First();
                setReadStatus(notifId);
            }

            if (!string.IsNullOrEmpty(Id))
            {
                req = (from r in db.EmployeeRequest where r.Id == Guid.Parse(Id) select r).First();
                setReadStatus(req.NotifId);
            }

            req.From = db.Employee.Find(Guid.Parse(req.From)).FullName;

            ViewData["req"] = req;

            return View();
        }

        [Authorize]
        public IActionResult AllRequest(int? perpage, int? page, int? order, string filter, int? status)
        {
            var reqs = (from r in db.EmployeeRequest select r).ToList();

            reqs = reqs.OrderByDescending(x => x.CreatedAt).ToList();

            int _perPage = 5;
            if (perpage.HasValue)
                _perPage = perpage.Value;

            var pager = new Pager(reqs.Count(), page, _perPage);

            var viewModel = new RequestPager
            {
                Items = reqs.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            if (status.HasValue)
                reqs = (from r in reqs where r.ApprovalStatus == status.Value select r).ToList();

            reqs.ForEach(
                x => x.From = db.Employee.Find(Guid.Parse(x.From)).FullName
                );

            ViewData["allReqs"] = reqs.ToList<EmployeeRequest>();


            return View(viewModel);
        }

        [Authorize]
        public IActionResult Approve(string Id, string notes)
        {
            var req = db.EmployeeRequest.Find(Guid.Parse(Id));
            req.ApprovalStatus = 1;
            req.Notes = notes;
            db.SaveChanges();

            var emp = db.Employee.Find(Guid.Parse(req.From));

            var mailBody = $@"<h1>Dear {emp.FullName},</h1><br>
                Permintaan {req.Subject} anda telah disetujui.<br>
                <h3>Salam, HR.</h3>";

            var sendThread = new Thread(() => MailController.sendMail("admin@tokoaneh.com", emp.Email,
                                                                      "Request Confirmation", mailBody));
            sendThread.Start();

            return Ok(req);
        }

        [Authorize]
        public IActionResult Reject(string Id, string notes)
        {
            var req = db.EmployeeRequest.Find(Guid.Parse(Id));
            req.ApprovalStatus = 2;
            req.Notes = notes;
            db.SaveChanges();

            var emp = db.Employee.Find(Guid.Parse(req.From));

            var mailBody = $@"<h1>Dear {emp.FullName},</h1><br>
                Permintaan {req.Subject} anda tidak disetujui dengan alasan :<br>
                {req.Notes}<br>
                Silahkan melakuakn revisi dan ajukan kembali permohonan anda.<br>
                <h3>Salam, HR.</h3>";

            var sendThread = new Thread(() => MailController.sendMail("admin@tokoaneh.com", emp.Email,
                                                                      "Request Confirmation", mailBody));
            sendThread.Start();

            return Ok(req);
        }

        private void registerRequest(string notifId, string sender, string to, string subject, string message, DateTime startDate, DateTime endDate)
        {
            var _req = new EmployeeRequest
            {
                NotifId = notifId,
                From = (from e in db.Employee where e.NIK == sender select e.Id.ToString()).First(),
                To = (from e in db.HRAdmin where e.Email == to select e.Id.ToString()).First(),
                Subject = subject,
                Message = message,
                DueDate = new DueDate { StartDate = startDate, EndDate = endDate},
                ApprovalStatus = 0,
                CreatedAt = DateTime.Now,
                Notes = "-"
            };

            db.EmployeeRequest.Add(_req);
            db.SaveChanges();
        }

        public class RequestPager
        {
            public IEnumerable<EmployeeRequest> Items { get; set; }
            public Pager Pager { get; set; }
        }

        private void setReadStatus(string notifId)
        {
            var _msg = db.NotificationLog.Find(Guid.Parse(notifId));
            _msg.OpenedAt = DateTime.Now;
            _msg.ReadStatus = "read";
            db.SaveChanges();
        }
    }
}