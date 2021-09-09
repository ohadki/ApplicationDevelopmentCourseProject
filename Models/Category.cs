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
        public int SoldProductsCount { get; set; } = 0;
        public IEnumerable<Product> products { get; set; }
    }
}
