using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.Models
{
    public class Attendance
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public DateTime ? ClockIn { get; set; }
        public string ClockInStatus { get; set; }
        public DateTime ? ClockOut { get; set; }
        public string ClockOutStatus { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EditedAt { get; set; }
    }
}
