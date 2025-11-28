using HRSystem.Domain.Entities;
using HRSystem.Domain.Entities.UserEntity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Infrastructure.Persistence
{
    public class HRDbContext : DbContext
    {
        public HRDbContext(DbContextOptions<HRDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(HRDbContext).Assembly);
        }
    }
}
