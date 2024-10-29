﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cleopatra.Domain;

namespace Cleopatra.Infrastructure
{
    public class SalonContext : DbContext
    {
        public SalonContext(DbContextOptions<SalonContext> options) : base(options) { }

        public DbSet<Client> Clients { get; set; }
        //public DbSet<Appointment> Appointments { get; set; }
        //public DbSet<Service> Services { get; set; }
        //public DbSet<Resource> Resources { get; set; }
    }
}