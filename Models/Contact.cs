using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [DisplayName("Name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(150, MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Your contact number :")]
        [Required(ErrorMessage = "A phone number is required.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Phone Number.")]
        public string Telephone { get; set; }
        [DisplayName("Message")]
        [Required(ErrorMessage = "Message is required")]
        [StringLength(250, MinimumLength = 1)]
        [DataType(DataType.Text)]
        public string Message { get; set; }
    }
}
