using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext: IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
        {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<EmailCode> EmailCodes {get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "Student",
                    NormalizedName = "STUDENT"
                },
                new IdentityRole
                {
                    Name = "Teacher",
                    NormalizedName = "TEACHER"
                },
                new IdentityRole
                {
                    Name = "Department",
                    NormalizedName = "DEPARTMENT"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            builder.Entity<EmailCode>(x => x.HasKey(ec => new {ec.Email, ec.Code}));
        }

    }
}