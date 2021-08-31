using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "ZipCode is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "ZipCode should be minimum 3 characters and a maximum of 50 characters")]

        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(50, MinimumLength = 3,
        ErrorMessage = "Address should be minimum 3 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        [Display(Name = "Address")]
        public string Address { get; set; }

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

        public decimal OrderTotal { get; set; }

        public DateTime OrderPlaced { get; set; }

        public string UserId { get; set; }
    }
}
