using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.Models
{
    public class EmployeeRequest
    {
        public Guid Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public int ApprovalStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ApprovedAt { get; set; }
        public DateTime RejectedAt { get; set; }
        public string Notes { get; set; }
    }
}
