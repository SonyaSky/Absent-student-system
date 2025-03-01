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
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<StudentGroup> StudentGroup { get; set; }
        public DbSet<Absence> Absences { get; set; }
        public DbSet<ConfirmationFile> ConfirmationFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<StudentGroup>(x => x.HasKey(s => new { s.StudentId, s.GroupId }));

            builder.Entity<StudentGroup>()
                .HasOne(u => u.Student)
                .WithMany(u => u.Groups)
                .HasForeignKey(s => s.StudentId);
            
            builder.Entity<StudentGroup>()
                .HasOne(u => u.Group)
                .WithMany(u => u.Students)
                .HasForeignKey(s => s.GroupId);

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

        }

    }
}