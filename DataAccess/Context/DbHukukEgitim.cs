﻿using Entity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class DbHukukEgitim:IdentityDbContext<AppUser,AppRole,int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("connection string");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppRole>().Property(r => r.ConcurrencyStamp).HasDefaultValueSql("NEWID()");
        }


        public DbSet<OnlineUser> onlineUsers { get; set; }
        public DbSet<Education> educations { get; set; }
        
        public DbSet<PurchasedTrainings> purchasedTrainings { get; set; }
        public DbSet<TrainerLessons> trainerLessons { get; set; }
        public DbSet<TrainingHours> trainingHours { get; set; }
    }
}
