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
        [Display(Name = "Order Total")]

        public decimal OrderTotal { get; set; }
        [Display(Name = "Date")]

        public DateTime OrderPlaced { get; set; }

        public IEnumerable<CartItem> Products { get; set; }

        public string ProductsString { get; set; }

        public string UserId { get; set; }

        public int BranchId { get; set; }

        public Branch Branch { get; set; }
    }
}
