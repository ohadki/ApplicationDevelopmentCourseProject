using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class Product
    {
        public Product()
        {
            this.ProductTags = new HashSet<ProductTag>();
        }

        [Key]
        public int Id { get; set; }

        [DisplayName("Quantity")]
        [Required(ErrorMessage = "Quantity is required")]
        [Range(0, 1000, ErrorMessage = "Value for {0} must be between {1} and {2}.")]

        public int Quantity { get; set; } = 0;

        [DisplayName("Product Name")]
        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100, MinimumLength = 3)]
        [RegularExpression("^.*[a-zA-Z]+.*$", ErrorMessage = "Product Name must have at least one letter")] // contains at least one letter
        public string Name { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, MinimumLength = 10)]
        [RegularExpression("^.*[a-zA-Z]+.*$", ErrorMessage = "Description must have at least one letter")] // contains at least one letter
        public string Description { get; set; }

        [DisplayName("Price")]
        [Required(ErrorMessage = "Price is required")]
        [Range(1,500,ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public decimal Price { get; set; }

        [DisplayName("Image")]
        [StringLength(255, MinimumLength = 5)]
        /* Image file extension validation
           1. It should start with a string of at least one character.
           2. It should not have any white space.
           3. It should be followed by a dot(.).
           4. It should be end with any one of the following extensions: jpg, jpeg, png, gif, bmp.
         */
        [RegularExpression("([^\\s]+(\\.(?i)(jpe?g|png|gif|bmp))$)", ErrorMessage = "Bad image format")]
        public string? Image { get; set; } 

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public virtual ICollection<ProductTag> ProductTags { get; set; }

        public string ProductTagsString { get; set; }
    }
}
