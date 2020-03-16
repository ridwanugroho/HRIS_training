using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using HRIS.Data;
using HRIS.Models;
using Microsoft.AspNetCore.Authorization;

namespace HRIS.Controllers
{
    public class BroadcastController : Controller
    {
        private AppDbContext db;

        public BroadcastController(AppDbContext db)
        {
            this.db = db;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult SendBroadcast(string title, string subject, string message)
        {
            var addresses = (from e in db.Employee where e.DataStatus != 0 && e.Email != string.Empty select e.Email).ToList();

            var sendThread = new Thread(() => sendTask(addresses, title, subject, message));
            sendThread.Start();

            return RedirectToAction("BroadcastConfirmation");

        }

        public IActionResult BroadcastConfirmation()
        {
            return View();
        }

        private void sendTask(List<string> addresses, string title, string subject, string message)
        {
            var mailBody = $@"<h1>{title},</h1><br>
                Dear All, <br>
                {message}<br>
                <h3>Salam, HR.</h3>";

            foreach (var addr in addresses)
            {
                try
                {
                    MailController.sendMail("admin@tokoaneh.com", addr, subject, mailBody);
                }
                catch
                {
                    return;
                }
            }
        }
    }
}