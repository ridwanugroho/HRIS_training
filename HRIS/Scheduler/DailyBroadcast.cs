using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using HRIS.Models;
using HRIS.Data;
using HRIS.Controllers;

namespace HRIS.Scheduler
{
    public class DailyBroadcast : IHostedService
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DailyBroadcast(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Start background task daily broadcast");

            Timer timer = new Timer(EventChecker, null, TimeSpan.Zero, TimeSpan.FromHours(1));

            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Stop background process");

            return Task.CompletedTask;
        }

        public void EventChecker(object state)
        {
            var t = DateTime.Now.ToString("HH:mm").Substring(0, 2);

            if (t == "01")
            {
                var emps = getBirthEmps();
                if (emps != null)
                {
                    foreach (var e in emps)
                    {
                        var mailBody = $@"<h1>Selamat Ulang tahun,{e.FullName} ,</h1><br>
                        Selamat ulang tahun, semoda barokah sisa umurnya :)<br>
                        <h3>Salam, HR.</h3>";
                        MailController.sendMail("admin@tokoaneh.com", e.Email, "BirthdarGreet", mailBody);
                    }
                }

                var evt = getUpcommingEvents();
                if(evt != null)
                {
                    var scope = _serviceScopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                    var empsAddress = from e in db.Employee select e.Email;

                    foreach(var e in evt)
                    {
                        foreach(var a in empsAddress)
                        {
                            var mailBody = $@"<h1>{e.Title}</h1><br>
                            {e.Detail}<br>
                            <h3>Salam, HR.</h3>";
                            MailController.sendMail("admin@tokoaneh.com", a, "BirthdarGreet", mailBody);
                        }
                    }
                }

            }
        }

        private List<Employee> getBirthEmps()
        {
            var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var emps = (from e in db.Employee where e.DOB.Date == DateTime.Now.Date select e).ToList();

            var temp = from e in db.Employee select e;

            if (emps.Count()! > 0)
                return null;

            else
                return emps;
        }

        private List<Event> getUpcommingEvents()
        {
            var scope = _serviceScopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var evt = (from e in db.Event where e.StartDate.Date == DateTime.Now.Date.AddDays(3) select e).ToList();

            if (evt.Count()! > 0)
                return null;

            else
                return evt;
        }

    }


}