using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ApplicationDevelopmentCourseProject.Models;

namespace ApplicationDevelopmentCourseProject.Data
{
    public class ApplicationDevelopmentCourseProjectContext : DbContext
    {
        public ApplicationDevelopmentCourseProjectContext (DbContextOptions<ApplicationDevelopmentCourseProjectContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationDevelopmentCourseProject.Models.Product> Product { get; set; }

        public DbSet<ApplicationDevelopmentCourseProject.Models.ProductTag> ProductTag { get; set; }

        public DbSet<ApplicationDevelopmentCourseProject.Models.Order> Order { get; set; }

        public DbSet<ApplicationDevelopmentCourseProject.Models.Category> Category { get; set; }

        public DbSet<ApplicationDevelopmentCourseProject.Models.User> User { get; set; }

        public DbSet<ApplicationDevelopmentCourseProject.Models.UserAddress> UserAddress { get; set; }

        public DbSet<ApplicationDevelopmentCourseProject.Models.Branch> Branch { get; set; }

        public DbSet<ApplicationDevelopmentCourseProject.Models.Contact> Contact { get; set; }

        
    }
}
