using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class UserAddress
    {

        [Key]
        public int UserAddressId { get; set; }
        public string AddressLine1 { get; set; }
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "AddressLine2 should be minimum 3 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        [Display(Name = "Address 2")]
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
        [Display(Name = "Phone")]
        [Required(ErrorMessage = "A phone number is required.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        public string ContactNumber { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public string GetUserAddress()
        {
            return AddressLine1 + ", " + City + ", " + Country;
        }
    }
}
