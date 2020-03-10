using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.Models
{
    public class HRAdmin
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Employee Identities { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public int DataStatus { get; set; }
    }
}
