using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.Models
{
    public class Role
    {
        public int Status { get; set; }
        public string Name { get; set; }
        public string Division { get; set; }
        public string SubDivision { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
    }
}
