using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.Models
{
    public class EmployeeRequest
    {
        public Guid Id { get; set; }
        public string NotifId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        [JsonIgnore]
        public string _dueDate { get; set; }
        public int ApprovalStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ApprovedAt { get; set; }
        public DateTime RejectedAt { get; set; }
        public string Notes { get; set; }

        [NotMapped]
        public DueDate DueDate
        {
            get { return _dueDate == null ? null : JsonConvert.DeserializeObject<DueDate>(_dueDate); }
            set { _dueDate = JsonConvert.SerializeObject(value); }
        }
    }

    public class DueDate
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
