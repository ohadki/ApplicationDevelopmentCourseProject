using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class Branch
    {
        [DisplayName("Id")]
        [Required(ErrorMessage = "Id is required")]
        [Range(1,100, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Key]
        public int Id { get; set; }
        [DisplayName("Branch Name")]
        [Required(ErrorMessage = "Branch Name is required")]
        [StringLength(150, MinimumLength = 1)]
        public string BranchName { get; set; }
        [DisplayName("Address")]
        [Required(ErrorMessage = "Address is required")]
        [StringLength(150, MinimumLength = 1)]
        public string Address { get; set; }
        [DisplayName("X - Coordinate")]
        [Required(ErrorMessage = "X Coordinate is required")]
        [Range(-180, 180, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double XCoordinate { get; set; }
        [DisplayName("Y - Coordinate")]
        [Required(ErrorMessage = "Y Coordinate is required")]
        [Range(-180, 180, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public double YCoordinate { get; set; }
    }
}
