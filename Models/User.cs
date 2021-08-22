using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class User
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Userame is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Userame should be minimum 3 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Id is required")]
        [StringLength(9, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 9)]
        public string Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "First Name should be minimum 3 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Last Name should be minimum 3 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Address should be minimum 3 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        public string AddressLine1 { get; set; }
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "AddressLine2 should be minimum 3 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        public string AddressLine2 { get; set; }
        [Required(ErrorMessage = "City is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "City should be minimum 3 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        public string City { get; set; }
        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Country should be minimum 3 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        public string Country { get; set; }
        [Display(Name = "Your contact number :")]
        [Required(ErrorMessage = "A phone number is required.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        public string ContactNumber { get; set; }
        public DateTime MemberSince { get; set; } = DateTime.Now;
        public IEnumerable<Order> Orders { get; set; }
    }
}
