using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AIEnterpriseCommandCenter.Domain.Entities;
using AIEnterpriseCommandCenter.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AIEnterpriseCommandCenter.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>()
                .Property(e => e.Salary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ProjectEmployee>()
         .HasOne(pe => pe.Project)
         .WithMany(p => p.ProjectEmployees)
         .HasForeignKey(pe => pe.ProjectId)
         .OnDelete(DeleteBehavior.Cascade);

            // Employee -> ProjectEmployees
            modelBuilder.Entity<ProjectEmployee>()
                .HasOne(pe => pe.Employee)
                .WithMany(e => e.ProjectEmployees)
                .HasForeignKey(pe => pe.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Project Manager
            modelBuilder.Entity<Project>()
                .HasOne(p => p.Manager)
                .WithMany()
                .HasForeignKey(p => p.ManagerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Business Entities
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Asset> Assets { get; set; }

        public DbSet<ServiceTicket> ServiceTickets { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectEmployee> ProjectEmployees { get; set; }

        public DbSet<Notification> Notifications { get; set; }
    }
}
