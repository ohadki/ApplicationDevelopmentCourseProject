using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public decimal Balance { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime MemberSince { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
}
