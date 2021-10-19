using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public enum UserType
    {
        Client,
        Admin
    }

    public class User
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Userame is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Userame should be minimum 3 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Password must be between 5 and 255 characters", MinimumLength = 5)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Id is required")]
        [StringLength(9, ErrorMessage = "Id must be 9 characters", MinimumLength = 9)]
        public string Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "First Name should be minimum 3 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Last Name should be minimum 3 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public virtual UserAddress Address { get; set; }
        public UserType Type { get; set; } = UserType.Client;
        public DateTime? MemberSince { get; set; } = DateTime.Now;
        public IEnumerable<Order> Orders { get; set; }

        public string GetFullName()
        {
            return this.FirstName + " " + this.LastName;
        }
    }
}
