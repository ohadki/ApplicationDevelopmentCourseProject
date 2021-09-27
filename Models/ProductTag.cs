using System;
using System.Collections.Generic;
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
        public string TagName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
