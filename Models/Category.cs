using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(50, MinimumLength = 1,
        ErrorMessage = "Category name should be minimum 1 characters and a maximum of 50 characters")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, MinimumLength = 1,
        ErrorMessage = "Description should be minimum 1 characters and a maximum of 200 characters")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        [StringLength(255, MinimumLength = 5)]
        /* Image file extension validation
           1. It should start with a string of at least one character.
           2. It should not have any white space.
           3. It should be followed by a dot(.).
           4. It should be end with any one of the following extensions: jpg, jpeg, png, gif, bmp.
         */
        [RegularExpression("([^\\s]+(\\.(?i)(jpe?g|png|gif|bmp))$)", ErrorMessage = "Bad image format")]
        public string ImageUrl { get; set; }
        public IEnumerable<Product> products { get; set; }
    }
}
