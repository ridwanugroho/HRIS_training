using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using HRIS.Data;
using HRIS.Controllers;
using HRIS.Models;

namespace HRIS.SignalR
{
    public class ChatHub : Hub
    {
        public readonly static ConnectionMapping<string> _connections = new ConnectionMapping<string>();
        private AppDbContext db;

        public ChatHub( AppDbContext appDbContex)
        {
            this.db = appDbContex;
        }

        public void SendChatMessage(string from, string to, string subject, string message)
        {
            var msg = new 
            {
                id = from,
                message = message
            };

            foreach (var connectionId in _connections.GetConnections(to))
                Clients.Client(connectionId).SendAsync("GotMessage", msg);

            var _msg = new NotificationLog()
            {
                Sender = from,
                Subject = subject,
                Message = message,
                CreatedAt = DateTime.Now,
            };

            db.NotificationLog.Add(_msg);
            db.SaveChanges();
        }

        public override Task OnConnectedAsync()
        {
            string name = Context.UserIdentifier;
            string conID = Context.ConnectionId;
            Console.WriteLine("{0}  :  {1}", name, conID);
            return base.OnConnectedAsync();
        }

        public void removeConId(string userId)
        {
            _connections.Remove(userId, Context.ConnectionId);
        }

        public void bindConId(string UserId)
        {
            if (!_connections.GetConnections(UserId).Contains(Context.ConnectionId))
                _connections.Add(UserId, Context.ConnectionId);
        }
    }
}