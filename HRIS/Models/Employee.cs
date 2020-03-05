﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.Models
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public string Address { get; set; }
    }
}
