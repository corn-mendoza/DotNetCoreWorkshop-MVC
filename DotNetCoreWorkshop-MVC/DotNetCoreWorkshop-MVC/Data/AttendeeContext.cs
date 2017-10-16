using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNetCoreWorkshop_MVC.Models;

    public class AttendeeContext : DbContext
    {
        public AttendeeContext (DbContextOptions<AttendeeContext> options)
            : base(options)
        {
        }

        public DbSet<DotNetCoreWorkshop_MVC.Models.AttendeeModel> AttendeeModel { get; set; }
    }
