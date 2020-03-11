using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.Models
{
    public class NotificationLog
    {
        public Guid Id { get; set; }
        public string Sender { get; set; }
        public string Reciever { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime OpenedAt { get; set; }
        public string ReadStatus { get; set; }
    }
}
