using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class ProductTag
    {
        public ProductTag()
        {
            this.Products = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }

        [DisplayName("Tag Name")]
        [Required(ErrorMessage = "Tag Name is required")]
        [StringLength(100, MinimumLength = 3)]
        [RegularExpression("^.*[a-zA-Z]+.*$", ErrorMessage = "Product Name must have at least one letter")] // contains at least one letter
        public string TagName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
