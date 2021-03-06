﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using HRIS.Models;

namespace HRIS.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Employee> Employee { get; set; }
        public DbSet<HRAdmin> HRAdmin { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Applicant> Applicant { get; set; }
        public DbSet<NotificationLog> NotificationLog { get; set; }
        public DbSet<EmployeeRequest> EmployeeRequest { get; set; }
        public DbSet<Event> Event { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
