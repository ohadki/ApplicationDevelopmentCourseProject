using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
